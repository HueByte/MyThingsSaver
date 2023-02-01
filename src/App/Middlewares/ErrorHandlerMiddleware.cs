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

        public async Task InvokeAsync(HttpContext context, ILogger<ErrorHandlerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var result = GetExceptionResponse(ex);

                if (ex is HandledException || ex is HandledExceptionList)
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                else
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

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