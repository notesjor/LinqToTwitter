﻿using System;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class StreamingMeta
    {
        /// <summary>
        /// Twitter server time when the request was sent
        /// </summary>
        [JsonPropertyName("sent")]
        public DateTime Sent { get; init; }

        /// <summary>
        /// Statistics on created or deleted, which ever applies for the request
        /// </summary>
        [JsonPropertyName("summary")]
        public StreamingMetaSummary? Summary { get; init; }
    }
}
