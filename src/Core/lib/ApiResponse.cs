using Microsoft.AspNetCore.Mvc;
using MTS.Common.ApiResonse;

namespace MTS.Core.lib
{
    public static class ApiResponse
    {
        public static IActionResult Empty()
        {
            BaseApiResponse<object> result = new(default, null!, true);
            return new OkObjectResult(result);
        }

        public static IActionResult Data<T>(T? data) where T : class
        {
            BaseApiResponse<T> result = new(data);
            return Create(result);
        }

        public static IActionResult ValueType<T>(T data) where T : struct
        {
            if (!typeof(T).IsPrimitive) throw new Exception($"{data.GetType()} is not primitive data type");

            BaseApiResponse<T> result = new(data);
            return Create(result);
        }

        public static IActionResult Create<T>(BaseApiResponse<T> result)
        {
            return new OkObjectResult(result);
        }
    }
}