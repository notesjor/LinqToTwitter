using System.Text.Json.Serialization;

namespace LinqToTwitter.Common
{
    public class TwitterPollOption
    {
        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }
    }
}
