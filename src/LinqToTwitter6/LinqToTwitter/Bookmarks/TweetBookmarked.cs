using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TweetBookmarked
    {
        [JsonPropertyName("bookmarked")]
        public bool Bookmarked { get; set; }
    }
}