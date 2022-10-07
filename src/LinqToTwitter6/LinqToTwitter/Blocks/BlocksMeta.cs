using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class BlocksMeta
    {
        [JsonPropertyName("result_count")]
        public int ResultCount { get; set; }
    }
}