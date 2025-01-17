﻿using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class LikeMeta
    {
        [JsonPropertyName("result_count")]
        public int ResultCount { get; set; }

        [JsonPropertyName("next_token")]
        public string? NextToken { get; set; }
    }
}