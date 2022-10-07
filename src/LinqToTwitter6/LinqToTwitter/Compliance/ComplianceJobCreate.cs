using System.Text.Json.Serialization;

namespace LinqToTwitter.Compliance
{
    public class ComplianceJobCreate
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("resumable")]
        public bool Resumable { get; set; }

    }
}
