using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TweetDeleted
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; init; }
    }
}