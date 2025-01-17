﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter.Common
{
    public class TwitterPoll
    {
        /// <summary>
        /// Number of minutes to run poll
        /// </summary>
        [JsonPropertyName("duration_minutes")]
        public int DurationMinutes { get; set; }

        /// <summary>
        /// When the poll ends
        /// </summary>
        [JsonPropertyName("end_datetime")]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Poll ID
        /// </summary>
        [JsonPropertyName("id")]
        public string? ID { get; set; }

        /// <summary>
        /// Poll options
        /// </summary>
        [JsonPropertyName("options")]
        public List<TwitterPollOption>? Options { get; set; }

        /// <summary>
        /// Whether the poll is open or closed
        /// </summary>
        [JsonPropertyName("voting_status")]
        public string? VotingStatus { get; set; }
    }
}
