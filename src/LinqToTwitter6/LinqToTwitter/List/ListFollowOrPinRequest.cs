using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class ListFollowOrPinRequest
    {
        [JsonPropertyName("list_id")]
        public string? ListID { get; set; }
    }
}