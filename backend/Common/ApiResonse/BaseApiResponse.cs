using System.Collections.Generic;

namespace Common.ApiResonse
{
    public class BaseApiResponse<T>
    {
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSuccess { get; set; }
    }
}