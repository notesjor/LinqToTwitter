﻿using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public record ListResponseData
    {
        [JsonPropertyName("id")]
        public string? ID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("updated")]
        public bool Updated { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("is_member")]
        public bool IsMember { get; set; }

        [JsonPropertyName("following")]
        public bool Following { get; set; }

        [JsonPropertyName("pinned")]
        public bool Pinned { get; set; }
    }
}