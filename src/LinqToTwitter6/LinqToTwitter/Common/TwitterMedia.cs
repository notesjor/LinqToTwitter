using System.Text.Json.Serialization;

namespace LinqToTwitter.Common
{
    /// <summary>
    /// Represents a Media object, such as gif, photo, or video
    /// </summary>
    public class TwitterMedia
    {
        /// <summary>
        /// Alt text to display with an image
        /// </summary>
        [JsonPropertyName("alt_text")]
        public string? AltText { get; set; }

        /// <summary>
        /// Milliseconds duration for videos
        /// </summary>
        [JsonPropertyName("duration_ms")]
        public int DurationMS { get; set; }

        /// <summary>
        /// Height in pixels
        [JsonPropertyName("height")]
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Media ID - Matches MediaKey in TweetAttachments
        /// </summary>
        [JsonPropertyName("media_key")]
        public string? MediaKey { get; set; }

        // TODO
        [JsonPropertyName("non_public_metrics")]
        public object? NonPublicMetrics { get; set; }

        // TODO
        [JsonPropertyName("organic_metrics")]
        public object? OrganicMetrics { get; set; }

        /// <summary>
        /// URL to animated GIF and video preview image
        /// </summary>
        [JsonPropertyName("preview_image_url")]
        public string? PreviewImageUrl { get; set; }

        // TODO
        [JsonPropertyName("promoted_metrics")]
        public object? PromotedMetrics { get; set; }

        // TODO
        [JsonPropertyName("public_metrics")]
        public object? PublicMetrics { get; set; }

        /// <summary>
        /// Type of media - e.g. gif, photo, or video
        /// </summary>
        [JsonConverter(typeof(TweetMediaTypeConverter))]
        [JsonPropertyName("type")]
        public TweetMediaType Type { get; set; }

        /// <summary>
        /// URL to photo preview image
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Width in pixels
        /// </summary>
        [JsonPropertyName("width")]
        public int Width { get; set; }
    }
}
