using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    /// <summary>
    /// Entity URL details
    /// </summary>
    public class TweetEntityUrl
    {
        /// <summary>
        /// From Twitter Card description (in HTML header)
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// How URL appears in the tweet
        /// </summary>
        [JsonPropertyName("display_url")]
        public string? DisplayUrl { get; set; }

        /// <summary>
        /// Ending position of URL
        /// </summary>
        [JsonPropertyName("end")]
        public int End { get; set; }

        /// <summary>
        /// URL that user typed, may or may not be shortened URL
        /// </summary>
        [JsonPropertyName("expanded_url")]
        public string? ExpandedUrl { get; set; }

        /// <summary>
        /// Details on images (<see cref="TweetEntityImage"/>) attached to tweet
        /// </summary>
        [JsonPropertyName("images")]
        public List<TweetEntityImage>? Images { get; set; }

        /// <summary>
        /// Starting position of URL
        /// </summary>
        [JsonPropertyName("start")]
        public int Start { get; set; }

        /// <summary>
        /// HTTP status from unwind
        /// </summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }

        /// <summary>
        /// From Twitter Card title (in HTML header)
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Final destination URL, after following intermediate shorteners (if any)
        /// </summary>
        [JsonPropertyName("unwound_url")]
        public string? UnwoundUrl { get; set; }

        /// <summary>
        /// Twitter's t.co URL
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
