using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    /// <summary>
    /// Describes a place, like a city or point of interest
    /// </summary>
    public class TwitterPlace
    {
        /// <summary>
        /// IDs of places containing this place
        /// </summary>
        [JsonPropertyName("contained_within")]
        public List<string>? ContainedWithin { get; set; }

        /// <summary>
        /// Full-length country name
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// ISO Alpha-2 country code
        /// </summary>
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        /// <summary>
        /// Full name of place
        /// </summary>
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }

        /// <summary>
        /// Place details
        /// </summary>
        [JsonPropertyName("geo")]
        public Geo? Geo { get; set; }

        /// <summary>
        /// Unique ID for this place
        /// </summary>
        [JsonPropertyName("id")]
        public string? ID { get; set; }

        /// <summary>
        /// Short name of place
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Type of place - e.g. city or point of interest
        /// </summary>
        [JsonPropertyName("place_type")]
        public string? PlaceType { get; set; }
    }
}
