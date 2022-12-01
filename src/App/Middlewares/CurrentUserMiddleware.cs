using System.Security.Claims;
using MTS.Core.Services.CurrentUser;

namespace MTS.App.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;
        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
        {
            currentUserService.UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _next(context);
        }
    }

    public static class CurrentUserExtensions
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
            => app.UseMiddleware<CurrentUserMiddleware>();
    }
}