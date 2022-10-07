using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class DeleteStreamingFilterRules
    {
        [JsonPropertyName("delete")]
        public DeleteIds? Delete { get; set; }
    }
}
