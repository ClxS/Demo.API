using Microsoft.AspNetCore.Diagnostics;

namespace Demo.API.Endpoints
{
    internal static class ErrorEndpoints
    {
        internal static void MapErrorEndpoints(this WebApplication app)
        {
            app.Map("/errors", LogError);
        }

        internal static IResult LogError(HttpContext httpContext)
        {
            var context = httpContext.Features.Get<IExceptionHandlerFeature>();

            return Results.Json(new { ErrorMessage = context.Error.Message, StackTrace = context.Error.StackTrace  });
        }
    }
}