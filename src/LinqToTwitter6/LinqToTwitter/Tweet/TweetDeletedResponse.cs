using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TweetDeletedResponse
    {
        [JsonPropertyName("data")]
        public TweetDeleted? Data { get; init; }
    }
}