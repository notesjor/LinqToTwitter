using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    /// <summary>
    /// Rule on filter stream
    /// </summary>
    public class MatchingRule
    {
        /// <summary>
        /// ID for rule
        /// </summary>
        [JsonPropertyName("id")]
        public string? ID { get; set; }

        /// <summary>
        /// Rule tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
    }

}
