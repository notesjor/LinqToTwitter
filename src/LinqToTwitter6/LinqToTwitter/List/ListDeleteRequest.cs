using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class ListDeleteRequest
    {
        [JsonPropertyName("id")]
        public string? ID { get; set; }
    }
}