using Microsoft.AspNetCore.Diagnostics;
using Pickuplay.DTOs;

namespace Pickuplay.Teams;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var response = new ApiResponse<object?>("error", exception.Message, null);

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true; // tells ASP.NET Core "I handled it, don't propagate further"
    }
}