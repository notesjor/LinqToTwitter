using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinqToTwitter
{
    public class AddStreamingFilterRules
    {
        [JsonPropertyName("add")]
        public List<StreamingAddRule>? Add { get; set; }
    }
}
