using System;
using Microsoft.AspNetCore.Mvc;
using MTS.Common.ApiResonse;

namespace MTS.Core.lib
{
    public static class CreateResponse
    {
        public static IActionResult Empty()
        {
            BaseApiResponse<object> result = new(default, null!, true);
            return new OkObjectResult(result);
        }

        public static IActionResult FromData<T>(T? data) where T : class
        {
            BaseApiResponse<T> result = new(data);
            return FromBaseApiResponse(result);
        }

        public static IActionResult FromPrimitive<T>(T data) where T : struct
        {
            if (!typeof(T).IsPrimitive) throw new Exception($"{data.GetType()} is not primitive data type");

            BaseApiResponse<T> result = new(data);
            return FromBaseApiResponse(result);
        }

        public static IActionResult FromBaseApiResponse<T>(BaseApiResponse<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result);

            return new BadRequestObjectResult(result);
        }
    }
}