using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class MuteResponseData
    {
        [JsonPropertyName("muting")]
        public bool Muting { get; set; }
    }
}
