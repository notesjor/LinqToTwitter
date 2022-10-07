﻿using LinqToTwitter.Common;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class Tweet
    {
        /// <summary>
        /// Attachments, such as polls, images, and videos
        /// </summary>
        [JsonPropertyName("attachments")]
        public TweetAttachments? Attachments { get; set; }

        /// <summary>
        /// Posting user's ID
        /// </summary>
        [JsonPropertyName("author_id")]
        public string? AuthorID { get; set; }

        /// <summary>
        /// Contextual metadata associated with tweet
        /// </summary>
        [JsonPropertyName("context_annotations")]
        public List<TweetContextAnnotation>? ContextAnnotations { get; set; }

        /// <summary>
        /// ID of conversation this tweet is part of (matches original tweet ID)
        /// </summary>
        [JsonPropertyName("conversation_id")]
        public string? ConversationID { get; set; }

        /// <summary>
        /// When tweeted
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Extracted parts of the tweet, like hashtags, urls, etc.
        /// </summary>
        [JsonPropertyName("entities")]
        public TweetEntities? Entities { get; set; }

        /// <summary>
        /// Tweet location (if user provided)
        /// </summary>
        [JsonPropertyName("geo")]
        public TweetGeo? Geo { get; set; }

        /// <summary>
        /// ID of this tweet
        /// </summary>
        [JsonPropertyName("id")]
        public string? ID { get; set; }

        /// <summary>
        /// If replying, this is the ID of the user being replied to
        /// </summary>
        [JsonPropertyName("in_reply_to_user_id")]
        public string? InReplyToUserID { get; set; }

        /// <summary>
        /// BCP47 Language tag - https://tools.ietf.org/html/bcp47
        /// </summary>
        [JsonPropertyName("lang")]
        public string? Language { get; set; }

        // TODO: finish implementation - requires permissions to access
        /// <summary>
        /// Non-Public Metrics
        /// </summary>
        // TODO: finish implementation - requires permissions to access
        [JsonPropertyName("non_public_metrics")]
        public object? NonPublicMetrics { get; set; }

        /// <summary>
        /// Organic Metrics
        /// </summary>
        // TODO: finish implementation - requires permissions to access
        [JsonPropertyName("organic_metrics")]
        public object? OrganicMetrics { get; set; }

        /// <summary>
        /// Media or links might reveal sensitive information
        /// </summary>
        [JsonPropertyName("possibly_sensitive")]
        public bool? PossiblySensitive { get; set; }

        /// <summary>
        /// Promoted content metrics
        /// </summary>
        // TODO: finish implementation - requires permissions to access
        [JsonPropertyName("promoted_metrics")]
        public object? PromotedMetrics { get; set; }

        /// <summary>
        /// Public metrics
        /// </summary>
        [JsonPropertyName("public_metrics")]
        public TweetPublicMetrics? PublicMetrics { get; set; }

        /// <summary>
        /// Any other tweets that this one refers to
        /// </summary>
        [JsonPropertyName("referenced_tweets")]
        public List<TweetReference>? ReferencedTweets { get; set; }

        /// <summary>
        /// Who can reply to tweet
        /// </summary>
        [JsonPropertyName("reply_settings")]
        public TweetReplySettings ReplySettings { get; set; }

        /// <summary>
        /// The application sending the tweet
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        /// <summary>
        /// User's tweet text
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>
        /// Information regarding a request to withhold information
        /// </summary>
        [JsonPropertyName("withheld")]
        public TwitterWithheld? Withheld { get; set; }
    }
}