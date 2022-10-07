using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class CountsMeta
    {
        [JsonPropertyName("total_tweet_count")]
        public int TotalTweetCount { get; set; }
    }
}
