using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class ListMemberRequest
    {
        [JsonPropertyName("user_id")]
        public string? UserID { get; set; }
    }
}