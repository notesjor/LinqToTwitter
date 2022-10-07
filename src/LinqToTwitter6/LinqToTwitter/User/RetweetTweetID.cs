using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class RetweetTweetID
    {
        [JsonPropertyName("tweet_id")]
        public string? TweetID { get; init; }
    }
}