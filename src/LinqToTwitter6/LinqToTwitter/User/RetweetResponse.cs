using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class RetweetResponse
    {
        [JsonPropertyName("data")]
        public RetweetResponseData? Data { get; init; }
    }
}