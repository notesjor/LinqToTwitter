using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter.Common
{
    public class TwitterInclude
    {
        [JsonPropertyName("users")]
        public List<TwitterUser>? Users { get; set; }

        [JsonPropertyName("tweets")]
        public List<Tweet>? Tweets { get; set; }

        [JsonPropertyName("places")]
        public List<TwitterPlace>? Places { get; set; }

        [JsonPropertyName("polls")]
        public List<TwitterPoll>? Polls { get; set; }

        [JsonPropertyName("media")]
        public List<TwitterMedia>? Media { get; set; }
    }
}
