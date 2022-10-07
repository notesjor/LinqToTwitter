using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class DeleteIds
    {
        [JsonPropertyName("ids")]
        public List<string>? Ids { get; set; }
    }
}
