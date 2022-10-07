using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class SpaceResponseData
    {
        [JsonPropertyName("muting")]
        public bool Muting { get; set; }
    }
}
