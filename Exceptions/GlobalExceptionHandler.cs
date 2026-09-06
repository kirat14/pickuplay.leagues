using Microsoft.AspNetCore.Diagnostics;

using Pickuplay.DTOs;
using Pickuplay.Teams.Exceptions;

namespace Pickuplay.Teams;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {

            UnauthorizedAccessException ex => (StatusCodes.Status401Unauthorized, ex.Message),
            AlreadyJoinedLeagueException ex => (StatusCodes.Status409Conflict, ex.Message),
            TeamFullException ex => (StatusCodes.Status409Conflict, ex.Message),
            TeamNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            LeagueNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            LeagueTeamEntryNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            LeagueRegistrationPeriodException ex => (StatusCodes.Status409Conflict, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (exception is DomainException or UnauthorizedAccessException)
        {
            _logger.LogWarning("Pickuplay error [{StatusCode}]: {Message}", statusCode, message);
        }
        else
        {
            _logger.LogError(exception, "An unexpected error occurred: {Message}", message);
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new ApiResponse<object?>("error", message, null);

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true; // tells ASP.NET Core "I handled it, don't propagate further"
    }
}