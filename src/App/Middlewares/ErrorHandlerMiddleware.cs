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
                logger.LogError(ex, "Error Message: ");
                var result = await GetExceptionResponse(context, ex);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // temp
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(result);
            }
        }

        public Task<BaseApiResponse<object>> GetExceptionResponse(HttpContext context, Exception exception)
        {
            BaseApiResponse<object> errorResult = exception switch
            {
                EndpointException => new()
                {
                    Data = default,
                    Errors = new List<string>() { exception.Message }!,
                    IsSuccess = false
                },
                EndpointExceptionList list => new()
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

            return Task.FromResult(errorResult);
        }
    }
}