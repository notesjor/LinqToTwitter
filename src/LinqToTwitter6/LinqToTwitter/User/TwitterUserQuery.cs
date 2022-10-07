using LinqToTwitter.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TwitterUserQuery
    {
        //
        // Query input fields
        //

        /// <summary>
        /// type of search, included for compatibility
        /// with other APIs
        /// </summary>
        public UserType Type { get; set; }

        /// <summary>
        /// Required for id queries - Up to 100 comma-separated IDs to search for
        /// </summary>
        public string? Ids { get; set; }

        /// <summary>
        /// Required for username queries - Up to 100 comma-separated usernames to search for
        /// </summary>
        public string? Usernames { get; set; }

        /// <summary>
        /// Comma-separated list of expansion fields
        /// </summary>
        public string? Expansions { get; set; }

        /// <summary>
        /// User ID for following/follower queries
        /// </summary>
        public string? ID { get; set; }

        /// <summary>
        /// ID for queries that need users associated with a list
        /// </summary>
        public string? ListID { get; set; }

        /// <summary>
        /// Max number of tweets to return per requrest - default 100 - possible 1000
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
        /// ID of space to query for users
        /// </summary>
        public string? SpaceID { get; set; }

        /// <summary>
        /// Comma-separated list of fields to return in the Tweet object
        /// </summary>
        public string? TweetFields { get; set; }

        /// <summary>
        /// ID of tweet that users are associated with. e.g. users who liked a tweet
        /// </summary>
        public string? TweetID { get; set; }

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
        public List<TwitterUser>? Users { get; set; }

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
    }
}
