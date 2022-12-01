using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MTS.Common.ApiResonse
{
    public class BaseApiResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<string?>? Errors { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        public BaseApiResponse() { }
        public BaseApiResponse(T? data) : this(data, null!, true) { }
        public BaseApiResponse(T? data, List<string> errors, bool isSuccess)
        {
            Data = data;
            Errors = errors!;
            IsSuccess = isSuccess;
        }
    }
}