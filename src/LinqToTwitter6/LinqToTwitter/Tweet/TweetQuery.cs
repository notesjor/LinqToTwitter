using LinqToTwitter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TweetQuery
    {
        //
        // Query input fields
        //

        /// <summary>
        /// type of search, included for compatibility
        /// with other APIs - <see cref="TweetType"/>
        /// </summary>
        public TweetType Type { get; set; }

        /// <summary>
        /// UTC date/time to search to
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Comma-separated list of tweet types to exclude
        /// </summary>
        public string? Exclude { get; set; }

        /// <summary>
        /// Comma-separated list of expansion fields
        /// </summary>
        public string? Expansions { get; set; }

        /// <summary>
        /// Up to 100 comma-separated IDs to search for
        /// </summary>
        public string? Ids { get; set; }

        /// <summary>
        /// User ID for timeline queries
        /// </summary>
        public string? ID { get; set; }

        /// <summary>
        /// ID for list to get tweets from
        /// </summary>
        public string? ListID { get; set; }

        /// <summary>
        /// Max number of tweets to return per requrest - default 10 - possible 100
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the media object - <see cref="MediaField"/>
        /// </summary>
        public string? MediaFields { get; set; }

        /// <summary>
        /// If set, with token from previous response metadata, pages forward or backward
        /// </summary>
        public string? PaginationToken { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the place object - <see cref="PlaceField"/>
        /// </summary>
        public string? PlaceFields { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the poll object - <see cref="PollField"/>
        /// </summary>
        public string? PollFields { get; set; }

        /// <summary>
        /// returns tweets later than this ID
        /// </summary>
        public string? SinceID { get; set; }

        /// <summary>
        /// Date to search from
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// ID of space to query for tweets
        /// </summary>
        public string? SpaceID { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the Tweet object - <see cref="TweetField"/>
        /// </summary>
        public string? TweetFields { get; set; }

        /// <summary>
        /// returns tweets earlier than this ID
        /// </summary>
        public string? UntilID { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the User object - <see cref="UserField"/>
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
        /// Metadata with count and paging details
        /// </summary>
        [JsonPropertyName("meta")]
        public TweetMeta? Meta { get; set; }
    }
}
