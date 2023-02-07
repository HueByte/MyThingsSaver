using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using MTS.Common.ApiResonse;
using MTS.Core.Entities;

namespace MTS.App.Middlewares
{
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
            => app.UseMiddleware<ErrorHandlerMiddleware>();
    }

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var result = GetExceptionResponse(ex);

                context.Response.StatusCode = ex switch
                {
                    HandledException => (int)HttpStatusCode.OK,
                    HandledExceptionList => (int)HttpStatusCode.OK,
                    TokenException => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.BadRequest
                };

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(result);
            }
        }

        public BaseApiResponse<object> GetExceptionResponse(Exception exception)
        {
            BaseApiResponse<object> errorResult = exception switch
            {
                HandledException => new()
                {
                    Data = default,
                    Errors = new List<string>() { exception.Message }!,
                    IsSuccess = false
                },
                HandledExceptionList list => new()
                {
                    Data = default,
                    Errors = list!.ExceptionMessages!,
                    IsSuccess = false
                },
                TokenException => new()
                {
                    Data = default,
                    Errors = new List<string>() { exception.Message }!,
                    IsSuccess = false
                },
                _ => new()
                {
                    Data = default,
                    Errors = new() { exception.Message }!,
                    IsSuccess = false
                },
            };

            return errorResult;
        }
    }
}