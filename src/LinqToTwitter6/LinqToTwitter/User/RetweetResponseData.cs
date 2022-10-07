using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class RetweetResponseData
    {
        [JsonPropertyName("retweeted")]
        public bool Retweeted { get; set; }
    }
}