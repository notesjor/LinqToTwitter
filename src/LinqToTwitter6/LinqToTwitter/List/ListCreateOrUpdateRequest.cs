using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class ListCreateOrUpdateRequest
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("private")]
        public bool Private { get; set; }
    }
}