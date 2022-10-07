﻿using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TwitterUserFollowResponseData
    {
        [JsonPropertyName("following")]
        public bool Following { get; set; }

        [JsonPropertyName("pending_follow")]
        public bool PendingFollow { get; set; }
    }
}