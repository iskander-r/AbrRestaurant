using Newtonsoft.Json;
using System.Collections.Generic;

namespace AbrRestaurant.MenuApi.Contracts.Shared
{
    public class ApiErrorResponse<T>
    {
        [JsonProperty("trace_id")]
        public string TraceId { get; set; }

        [JsonProperty("moment_utc")]
        public string MomentUtc { get; set; }

        [JsonProperty("error_descriptions")]
        public IEnumerable<string> ErrorDescriptions { get; set; }

        [JsonProperty("http_status_code")]
        public int HttpStatusCode { get; set; }

        [JsonProperty("error_object", NullValueHandling = NullValueHandling.Ignore)]
        public T ErrorObject { get; set; }
    }
}
