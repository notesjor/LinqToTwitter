﻿using LinqToTwitter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace LinqToTwitter
{
    /// <summary>
    /// for performing Twitter searches
    /// </summary>
    [XmlType(Namespace = "LinqToTwitter")]
    public class TwitterSearch
    {
        //
        // Input parameters
        //

        /// <summary>
        /// type of search, included for compatibility
        /// with other APIs
        /// </summary>
        public SearchType Type { get; set; }

        /// <summary>
        /// Date/Time to search to
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Comma-separated list of expansion fields
        /// </summary>
        public string? Expansions { get; set; }

        /// <summary>
        /// Maximum number of tweets to return
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the media object
        /// </summary>
        public string? MediaFields { get; set; }

        /// <summary>
        /// Provide this, when paging, to get the next page of results
        /// </summary>
        public string? NextToken { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the place object
        /// </summary>
        public string? PlaceFields { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the poll object
        /// </summary>
        public string? PollFields { get; set; }

        /// <summary>
        /// search query
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// Return tweets whose IDs are greater than this
        /// </summary>
        public string? SinceID { get; set; }

        /// <summary>
        /// Order to return tweets
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// Date/Time to start search
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the Tweet object
        /// </summary>
        public string? TweetFields { get; set; }

        /// <summary>
        /// Return tweets whose ids are less than this
        /// </summary>
        public string? UntilID { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the User object
        /// </summary>
        public string? UserFields { get; set; }

        //
        // Output results
        //

        /// <summary>
        /// Tweet data returned from the search
        /// </summary>
        [JsonPropertyName("data")]
        public List<Tweet>? Tweets { get; set; }

        /// <summary>
        /// If any errors occur, they'll show up here
        /// </summary>
        [JsonPropertyName("errors")]
        public List<TwitterError>? Errors { get; set; }

        /// <summary>
        /// Were there errors?
        /// </summary>
        public bool HasErrors { get => Errors?.Any() ?? false; }

        /// <summary>
        /// Populated when query includes expansion fields
        /// </summary>
        [JsonPropertyName("includes")]
        public TwitterInclude? Includes { get; set; }

        /// <summary>
        /// Tweet metadata returned from search
        /// </summary>
        [JsonPropertyName("meta")]
        public TwitterSearchMeta? Meta { get; set; }
    }
}
