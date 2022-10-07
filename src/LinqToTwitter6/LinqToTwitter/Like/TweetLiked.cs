using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TweetLiked
    {
        [JsonPropertyName("liked")]
        public bool Liked { get; init; }
    }
}