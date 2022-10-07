using LinqToTwitter.Common;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class ListResponse
    {
        [JsonPropertyName("data")]
        public ListResponseData? Data { get; set; }

        [JsonPropertyName("includes")]
        public TwitterInclude? Includes { get; set; }
    }
}
