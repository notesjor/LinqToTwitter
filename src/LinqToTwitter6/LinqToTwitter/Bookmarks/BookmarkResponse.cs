using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class BookmarkResponse
    {
        [JsonPropertyName("data")]
        public TweetBookmarked? Data { get; init; }
    }
}