﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter.Common;
using LinqToTwitter.Net;
using LinqToTwitter.OAuth;

namespace LinqToTwitter.Provider
{
    /// <summary>
    /// Logic that performs actual communication with Twitter
    /// </summary>
    internal partial class TwitterExecute : ITwitterExecute, IDisposable
    {
        internal const int DefaultReadWriteTimeout = 300000;
        internal const int DefaultTimeout = 100000;

        /// <summary>
        /// Gets or sets the object that can send authorized requests to Twitter.
        /// </summary>
        public IAuthorizer? Authorizer { get; set; }

        /// <summary>
        /// Timeout (milliseconds) for writing to request 
        /// stream or reading from response stream
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>
        /// Timeout (milliseconds) to wait for a server response
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets the most recent URL executed
        /// </summary>
        /// <remarks>
        /// This is very useful for debugging
        /// </remarks>
        public Uri? LastUrl { get; private set; }

        /// <summary>
        /// list of response headers from query
        /// </summary>
        public IDictionary<string, string>? ResponseHeaders { get; set; }

        /// <summary>
        /// Gets and sets HTTP UserAgent header
        /// </summary>
        public string? UserAgent
        {
            get
            {
                return Authorizer?.UserAgent;
            }
            set
            {
                if (Authorizer != null)
                    Authorizer.UserAgent =
                        string.IsNullOrWhiteSpace(value) ?
                            Authorizer.UserAgent :
                            value + ", " + Authorizer.UserAgent;
            }
        }

        /// <summary>
        /// Assign your TextWriter instance to receive LINQ to Twitter output
        /// </summary>
        public static TextWriter? Log { get; set; }

        readonly object streamingCallbackLock = new();

        /// <summary>
        /// Allows users to process content returned from stream
        /// </summary>
        public Func<StreamContent, Task>? StreamingCallbackAsync { get; set; }

        /// <summary>
        /// HttpClient instance being used in a streaming operation
        /// </summary>
        internal HttpClient? StreamingClient { get; set; }

        /// <summary>
        /// Set to true to close stream, false means stream is still open
        /// </summary>
        public bool IsStreamClosed { get; internal set; }

        /// <summary>
        /// Allows callers to cancel operation (where applicable)
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        readonly object asyncCallbackLock = new();

        /// <summary>
        /// supports testing
        /// </summary>
        public TwitterExecute(IAuthorizer authorizer)
        {
            Authorizer = authorizer ?? throw new ArgumentNullException("authorizedClient");
            Authorizer.UserAgent = Authorizer.UserAgent ?? L2TKeys.DefaultUserAgent;
        }

        /// <summary>
        /// Used in queries to read information from Twitter API endpoints.
        /// </summary>
        /// <param name="request">Request with url endpoint and all query parameters</param>
        /// <param name="reqProc">Request Processor for Async Results</param>
        /// <returns>XML Respose from Twitter</returns>
        public async Task<string> QueryTwitterAsync<T>(Request request, IRequestProcessor<T> reqProc)
        {
            WriteLog(request.FullUrl, nameof(QueryTwitterAsync));
            _ = Authorizer ?? throw new ArgumentNullException(nameof(Authorizer), $"{nameof(Authorizer)} is required.");


            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(request.FullUrl));

            Dictionary<string, string> parms = request.RequestParameters
                               .ToDictionary(
                                    key => key.Name,
                                    val => val.Value);
            var handler = new GetMessageHandler(this, parms, request.FullUrl, Authorizer.SupportsCompression);
            var client = new HttpClient(handler);

            if (Timeout != 0)
                client.Timeout = new TimeSpan(0, 0, 0, Timeout);

            var msg = await client.SendAsync(req, CancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync(msg).ConfigureAwait(false);
        }
  
        internal void SetAuthorizationHeader(string method, string url, IDictionary<string, string> parms, HttpRequestMessage req)
        {
            _ = Authorizer ?? throw new ArgumentNullException(nameof(Authorizer), $"{nameof(Authorizer)} is required.");

            var authStringParms = parms.ToDictionary(parm => parm.Key, elm => elm.Value);
            authStringParms.Add("oauth_consumer_key", Authorizer.CredentialStore?.ConsumerKey ?? string.Empty);
            authStringParms.Add("oauth_token", Authorizer.CredentialStore?.OAuthToken ?? string.Empty);

            string? authorizationString = Authorizer.GetAuthorizationString(method, url, authStringParms);

            req.Headers.Add("Authorization", authorizationString);
        }

        /// <summary>
        /// Performs a query on the Twitter Stream.
        /// </summary>
        /// <param name="request">Request with url endpoint and all query parameters.</param>
        /// <returns>
        /// Caller expects an JSON formatted string response, but
        /// real response(s) with streams is fed to the callback.
        /// </returns>
        public async Task<string> QueryTwitterStreamAsync(Request request)
        {
            WriteLog(request.FullUrl, nameof(QueryTwitterStreamAsync));
            _ = Authorizer ?? throw new ArgumentNullException(nameof(Authorizer), $"{nameof(Authorizer)} is required.");

            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(request.FullUrl));

            var parms = request.RequestParameters
                               .ToDictionary(
                                    key => key.Name,
                                    val => val.Value);
            Authorizer.SupportsCompression = false;
            var handler = new GetMessageHandler(this, parms, request.FullUrl, Authorizer.SupportsCompression);

            StreamingClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite)
            };

            var response = await StreamingClient.SendAsync(
                req, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            await TwitterErrorHandler.ThrowIfErrorAsync(response).ConfigureAwait(false);

            Stream? stream = null;
            MemoryStream? memStr = null;

            try
            {
                const int CarriageReturn = 0x0D;
                const int LineFeed = 0x0A;

                stream = await CreateStream(response).ConfigureAwait(false);
                memStr = new MemoryStream();

                byte[] readByte;

                while (stream.CanRead && !IsStreamClosed)
                {
                    readByte = new byte[1];
                    await stream.ReadAsync(readByte, 0, 1, CancellationToken).ConfigureAwait(false);
                    byte nextByte = readByte.SingleOrDefault();

                    CancellationToken.ThrowIfCancellationRequested();

                    if (IsStreamClosed) break;

                    if (nextByte == 0xff) break;

                    if (nextByte != CarriageReturn && nextByte != LineFeed)
                        memStr.WriteByte(nextByte);

                    if (nextByte == LineFeed)
                    {
                        int byteCount = (int)memStr.Length;
                        byte[] tweetBytes = new byte[byteCount];

                        memStr.Position = 0;
                        await memStr.ReadAsync(tweetBytes, 0, byteCount, CancellationToken).ConfigureAwait(false);

                        string tweet = Encoding.UTF8.GetString(tweetBytes, 0, byteCount);
                        var strmContent = new StreamContent(this, tweet);

                        if (StreamingCallbackAsync != null)
                            await StreamingCallbackAsync(strmContent).ConfigureAwait(false);

                        memStr.Dispose();
                        memStr = new MemoryStream();
                    }
                }
            }
            finally
            {
                stream?.Dispose();
                memStr?.Dispose();
            }

            IsStreamClosed = false;

            return "{}";
        }
 
        HttpRequestMessage ConfigureRequest(Request request)
        {
            _ = Authorizer ?? throw new ArgumentNullException(nameof(Authorizer), $"{nameof(Authorizer)} is required.");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, request.Endpoint);

            var parameters =
                string.Join("&",
                    (from parm in request.RequestParameters
                     select parm.Name + "=" + Url.PercentEncode(parm.Value))
                    .ToList());                            
            var content = new StringContent(parameters, Encoding.UTF8, "application/x-www-form-urlencoded");
            httpRequest.Content = content;

            var parms = request.RequestParameters
                               .ToDictionary(
                                    key => key.Name,
                                    val => val.Value);
            SetAuthorizationHeader(HttpMethod.Post.ToString(), request.FullUrl, parms, httpRequest);
            httpRequest.Headers.Add("User-Agent", UserAgent);
            httpRequest.Headers.ExpectContinue = false;

            if (Authorizer.SupportsCompression)
                httpRequest.Headers.AcceptEncoding.TryParseAdd("gzip");

            return httpRequest;
        }
 
        async Task<Stream> CreateStream(HttpResponseMessage response)
        {
            _ = Authorizer ?? throw new ArgumentNullException(nameof(Authorizer), $"{nameof(Authorizer)} is required.");

            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            if (Authorizer.SupportsCompression)
                return new GZipStream(stream, CompressionMode.Decompress);
            else
                return stream;
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        public void CloseStream()
        {
            IsStreamClosed = true;

            if (StreamingClient != null)
                StreamingClient.CancelPendingRequests();
        }

        /// <summary>
        /// Performs HTTP POST media byte array upload to Twitter.
        /// </summary>
        /// <param name="url">Url to upload to.</param>
        /// <param name="postData">Request parameters.</param>
        /// <param name="data">Image to upload.</param>
        /// <param name="name">Image parameter name.</param>
        /// <param name="fileName">Image file name.</param>
        /// <param name="contentType">Type of image: must be one of jpg, gif, or png.</param>
        /// <param name="reqProc">Request processor for handling results.</param>
        /// <returns>JSON response From Twitter.</returns>
        public async Task<string> PostImageAsync(string url, IDictionary<string, string> postData, byte[] data, string name, string fileName, string contentType, CancellationToken cancelToken)
        {
            WriteLog(url, nameof(PostImageAsync));

            var multiPartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(data);
            byteArrayContent.Headers.Add("Content-Type", contentType);
            multiPartContent.Add(byteArrayContent, name, fileName);

            var cleanPostData = new Dictionary<string, string>();

            foreach (var pair in postData)
            {
                if (pair.Value != null)
                {
                    cleanPostData.Add(pair.Key, pair.Value);
                    multiPartContent.Add(new StringContent(pair.Value), pair.Key);
                }
            }

            var handler = new PostMessageHandler(this, new Dictionary<string, string>(), url);
            var client = new HttpClient(handler);

            if (Timeout != 0)
                client.Timeout = new TimeSpan(0, 0, 0, Timeout);

            HttpResponseMessage msg = await client.PostAsync(url, multiPartContent, cancelToken).ConfigureAwait(false);

            return await HandleResponseAsync(msg).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs HTTP POST media byte array upload to Twitter.
        /// </summary>
        /// <param name="url">Url to upload to.</param>
        /// <param name="postData">Request parameters.</param>
        /// <param name="data">Image to upload.</param>
        /// <param name="name">Image parameter name.</param>
        /// <param name="fileName">Image file name.</param>
        /// <param name="contentType">Type of image: must be one of jpg, gif, or png.</param>
        /// <param name="reqProc">Request processor for handling results.</param>
        /// <param name="mediaCategory">
        /// Media category - possible values are tweet_image, tweet_gif, and tweet_video. 
        /// See this post on the Twitter forums: https://twittercommunity.com/t/media-category-values/64781/6
        /// </param>
        /// <param name="shared">True if can be used in multiple DM Events.</param>
        /// <param name="cancelToken">Cancellation token</param>
        /// <returns>JSON response From Twitter.</returns>
        public async Task<string> PostMediaAsync(string url, IDictionary<string, string> postData, byte[] data, string name, string fileName, string? contentType, string mediaCategory, bool shared, CancellationToken cancelToken)
        {
            WriteLog(url, nameof(PostMediaAsync));

            ulong mediaID = await InitAsync(url, data, postData, contentType, mediaCategory, shared, cancelToken).ConfigureAwait(false);

            await AppendChunksAsync(url, mediaID, data, name, fileName, contentType, cancelToken).ConfigureAwait(false);

            return await FinalizeAsync(url, mediaID, cancelToken).ConfigureAwait(false);
        }

        async Task<ulong> InitAsync(string url, byte[] data, IDictionary<string, string> postData, string? contentType, string mediaCategory, bool shared, CancellationToken cancelToken)
        {
            var multiPartContent = new MultipartFormDataContent
            {
                { new StringContent("INIT"), "command" },
                { new StringContent(contentType ?? string.Empty), "media_type" }
            };
            if (!string.IsNullOrWhiteSpace(mediaCategory))
                multiPartContent.Add(new StringContent(mediaCategory), "media_category");
            if (shared)
                multiPartContent.Add(new StringContent("true"), "shared");
            multiPartContent.Add(new StringContent(data.Length.ToString()), "total_bytes");

            foreach (var pair in postData)
            {
                if (pair.Value != null)
                    multiPartContent.Add(new StringContent(pair.Value), pair.Key);
            }

            var handler = new PostMessageHandler(this, new Dictionary<string, string>(), url);
            var client = new HttpClient(handler);

            if (Timeout != 0)
                client.Timeout = new TimeSpan(0, 0, 0, Timeout);

            HttpResponseMessage msg = await client.PostAsync(url, multiPartContent, cancelToken).ConfigureAwait(false);

            string response = await HandleResponseAsync(msg).ConfigureAwait(false);

            var media = JsonDocument.Parse(response);
            var mediaID = media.RootElement.GetProperty("media_id").GetUInt64();
            return mediaID;
        }

        async Task AppendChunksAsync(string url, ulong mediaID, byte[] data, string name, string fileName, string? contentType, CancellationToken cancelToken)
        {
            const int ChunkSize = 5000000;

            for (
                int segmentIndex = 0, skip = 0; 
                skip < data.Length; 
                segmentIndex++, skip = segmentIndex * ChunkSize)
            {
                byte[] chunk = data.Skip(skip).Take(ChunkSize).ToArray();

                var multiPartContent = new MultipartFormDataContent();

                var byteArrayContent = new ByteArrayContent(chunk);
                byteArrayContent.Headers.Add("Content-Type", contentType);
                multiPartContent.Add(byteArrayContent, name, fileName);

                multiPartContent.Add(new StringContent("APPEND"), "command");
                multiPartContent.Add(new StringContent(mediaID.ToString()), "media_id");
                multiPartContent.Add(new StringContent(segmentIndex.ToString()), "segment_index");

                var handler = new PostMessageHandler(this, new Dictionary<string, string>(), url);
                var client = new HttpClient(handler);

                if (Timeout != 0)
                    client.Timeout = new TimeSpan(0, 0, 0, Timeout);

                HttpResponseMessage msg = await client.PostAsync(url, multiPartContent, cancelToken).ConfigureAwait(false);

                await HandleResponseAsync(msg).ConfigureAwait(false);
            }
        }

        async Task<string> FinalizeAsync(string url, ulong mediaID, CancellationToken cancelToken)
        {
            var multiPartContent = new MultipartFormDataContent
            {
                { new StringContent("FINALIZE"), "command" },
                { new StringContent(mediaID.ToString()), "media_id" }
            };

            var handler = new PostMessageHandler(this, new Dictionary<string, string>(), url);
            var client = new HttpClient(handler);

            if (Timeout != 0)
                client.Timeout = new TimeSpan(0, 0, 0, Timeout);

            HttpResponseMessage msg = await client.PostAsync(url, multiPartContent, cancelToken).ConfigureAwait(false);

            return await HandleResponseAsync(msg).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs HTTP POST, with JSON payload, to Twitter.
        /// </summary>
        /// <param name="method">Delete, Post, or Put</param>
        /// <param name="url">URL of request.</param>
        /// <param name="postData">URL parameters to post.</param>
        /// <param name="postObj">Serializable payload object.</param>
        /// <param name="getResult">Callback for handling async Json response - null if synchronous.</param>
        /// <returns>JSON Response from Twitter - empty string if async.</returns>
        public async Task<string> SendJsonToTwitterAsync<T>(string method, string url, IDictionary<string, string> postData, T postObj, CancellationToken cancelToken)
        {
            WriteLog(url, nameof(SendJsonToTwitterAsync));

            var postJson = postObj == null ? "" : JsonSerializer.Serialize(postObj);
            var content = new StringContent(postJson, Encoding.UTF8, "application/json");

            var cleanPostData = new Dictionary<string, string>();
            foreach (var pair in postData)
                if (pair.Value != null)
                    cleanPostData.Add(pair.Key, pair.Value);
            var handler = new PostMessageHandler(this, cleanPostData, url);

            var client = new HttpClient(handler);

            if (Timeout != 0)
                client.Timeout = new TimeSpan(0, 0, 0, Timeout);

            HttpResponseMessage msg;

            if (method == HttpMethod.Post.ToString())
                msg = await client.PostAsync(url, content, cancelToken).ConfigureAwait(false);
            else if (method == HttpMethod.Delete.ToString())
                msg = await client.DeleteAsync(url, cancelToken).ConfigureAwait(false);
            else
                msg = await client.PutAsync(url, content, cancelToken).ConfigureAwait(false);

            return await HandleResponseAsync(msg).ConfigureAwait(false);
        }

        /// <summary>
        /// performs HTTP POST to Twitter
        /// </summary>
        /// <param name="method">Delete, Post, or Put</param>
        /// <param name="url">URL of request</param>
        /// <param name="postData">parameters to post</param>
        /// <param name="getResult">callback for handling async Json response - null if synchronous</param>
        /// <returns>Json Response from Twitter - empty string if async</returns>
        public async Task<string> PostFormUrlEncodedToTwitterAsync<T>(string method, string url, IDictionary<string, string?> postData, CancellationToken cancelToken)
        {
            WriteLog(url, nameof(PostFormUrlEncodedToTwitterAsync));

            var cleanPostData = new Dictionary<string, string>();

            var dataString = new StringBuilder();

            foreach (var pair in postData)
            {
                if (pair.Value != null)
                {
                    dataString.AppendFormat("{0}={1}&", pair.Key, Url.PercentEncode(pair.Value));
                    cleanPostData.Add(pair.Key, pair.Value);
                }
            }

            var content = new StringContent(dataString.ToString().TrimEnd('&'), Encoding.UTF8, "application/x-www-form-urlencoded");
            var handler = new PostMessageHandler(this, cleanPostData, url);
            var client = new HttpClient(handler);

            if (Timeout != 0)
                client.Timeout = new TimeSpan(0, 0, 0, Timeout);

            HttpResponseMessage msg;
            if (method == HttpMethod.Delete.ToString())
                msg = await client.DeleteAsync(url, cancelToken).ConfigureAwait(false);
            else
                msg = await client.PostAsync(url, content, cancelToken).ConfigureAwait(false);

            return await HandleResponseAsync(msg).ConfigureAwait(false);
        }
  
        async Task<string> HandleResponseAsync(HttpResponseMessage msg)
        {
            LastUrl = msg.RequestMessage?.RequestUri;

            ResponseHeaders =
                (from header in msg.Headers
                 select new
                 {
                     header.Key,
                     Value = string.Join(", ", header.Value)
                 })
                .ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value);

            await TwitterErrorHandler.ThrowIfErrorAsync(msg).ConfigureAwait(false);

            return await msg.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        static void WriteLog(string content, string currentMethod)
        {
            if (Log != null)
            {
                Log.WriteLine("--Log Starts Here--");
                Log.WriteLine("Query:" + content);
                Log.WriteLine("Method:" + currentMethod);
                Log.WriteLine("--Log Ends Here--");
                Log.Flush();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreamingCallbackAsync = null;

                if (Log != null)
                {
                    Log.Dispose();
                }
            }
        }
    }
}