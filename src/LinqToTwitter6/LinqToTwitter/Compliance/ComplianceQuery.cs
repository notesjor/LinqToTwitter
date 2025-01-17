﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class ComplianceQuery
    {
        //
        // Query input fields
        //

        /// <summary>
        /// type of compliance job query
        /// </summary>
        public ComplianceType Type { get; set; }

        /// <summary>
        /// ID for a single job query
        /// </summary>
        public string? ID { get; set; }

        /// <summary>
        /// Type of compliance job to query (tweets or users)
        /// </summary>
        public string? JobType { get; set; }

        /// <summary>
        /// Comma-separated list of job statuses
        /// </summary>
        public string? Status { get; set; }

        //
        // Output results
        //

        /// <summary>
        /// Compliance job data returned from the search
        /// </summary>
        [JsonPropertyName("data")]
        public List<ComplianceJob>? Jobs { get; set; }
    }
}
