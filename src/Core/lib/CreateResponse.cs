using System;
using System.Threading.Tasks;
using Common.ApiResonse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core.lib
{
    public static class CreateResponse
    {
        public static IActionResult Empty()
        {
            var result = new BaseApiResponse<object>(default, null, true);
            return new OkObjectResult(result);
        }

        public static IActionResult FromData<T>(T? data) where T : class
        {
            var result = new BaseApiResponse<T>(data);

            return FromBaseApiResponse(result);
        }

        public static IActionResult FromPrimitive<T>(T data) where T : struct
        {
            if (!typeof(T).IsPrimitive) throw new Exception($"{data.GetType()} is not primitive data type");

            var result = new BaseApiResponse<T>(data);

            return FromBaseApiResponse(result);
        }

        public static IActionResult FromBaseApiResponse<T>(BaseApiResponse<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result);
            else
                return new BadRequestObjectResult(result);
        }
    }
}