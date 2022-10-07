using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class StreamTweet
    {
        [JsonPropertyName("data")]
        public Tweet? Tweet { get; set; }

        [JsonPropertyName("matching_rules")]
        public List<MatchingRule>? MatchingRules { get; set; }
    }
}
