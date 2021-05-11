using AbrRestaurant.Domain.Errors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AbrRestaurant.MenuApi.Contracts.Shared
{
    public class ApiErrorResponse
    {
        [JsonProperty("trace_id")]
        public string TraceId { get; set; }

        [JsonProperty("moment_utc")]
        public string MomentUtc { get; set; }

        [JsonProperty("error_descriptions")]
        public IEnumerable<string> ErrorDescriptions { get; set; }

        [JsonProperty("http_status_code")]
        public int HttpStatusCode { get; set; }
    }

    public static class ApiErrorResponseFactory
    {
        public static ApiErrorResponse CreateFrom(
            BaseException domainException, string traceId)
        {
            return new ApiErrorResponse
            {
                TraceId = traceId,
                HttpStatusCode = domainException.AssociatedHttpStatusCode,
                ErrorDescriptions = domainException.ErrorDescriptions,
                MomentUtc = DateTime.UtcNow.ToString()
            };
        }
    }
}
