using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class BlockingResponseData
    {
        [JsonPropertyName("blocking")]
        public bool Blocking { get; set; }
    }
}
