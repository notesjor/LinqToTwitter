using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class TwitterUserTargetID
    {
        [JsonPropertyName("target_user_id")]
        public string? TargetUserID { get; set; }
    }
}