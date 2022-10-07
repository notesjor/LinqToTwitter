using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class LikedResponse
    {
        [JsonPropertyName("data")]
        public TweetLiked? Data { get; set; }
    }
}