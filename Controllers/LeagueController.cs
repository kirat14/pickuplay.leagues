using Microsoft.AspNetCore.Authorization; // gives us [Authorize] attribute
using Microsoft.AspNetCore.Mvc;
using Pickuplay.DTOs;
using Pickuplay.Services;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Controllers;

[ApiController]                    // enables automatic model validation and request binding
[Route("api/leagues")]               // base route: all endpoints here start with /api/teams
public class LeagueController : ControllerBase  // gives us Ok(), NotFound(), etc.
{
    public readonly ILeagueService _leagueService;

    public LeagueController(ILeagueService leagueService)
    {
        _leagueService = leagueService;
    }

    [HttpPost]                     // maps HTTP POST requests to this method
    [Authorize(Roles = "ADMIN, ORGANIZER")]                    // requires a valid JWT token to access this endpoint
    public IActionResult CreateTeam(CreateLeagueRequest request)
    {
        var userIdClaim = User.FindFirst("id")?.Value;

        if (!int.TryParse(userIdClaim, out var organizerId))
        {
            return Unauthorized("User ID could not be found in the token.");
        }

        League league = _leagueService.CreateLeague(request, organizerId);

        return Ok(new ApiResponse<LeagueResponse>("success", "League created successfully", new LeagueResponse(
            league.Id,
            league.Name,
            league.City,
            league.DateTime,
            league.Teams.Select(t => t.Name).ToList()
        )));
    }
}