using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TwitterUserFollowResponse
    {
        [JsonPropertyName("data")]
        public TwitterUserFollowResponseData? Data { get; init; }
    }
}