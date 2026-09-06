using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization; // gives us [Authorize] attribute
using Microsoft.AspNetCore.Mvc;

using Pickuplay.DTOs;
using Pickuplay.Mappers;
using Pickuplay.Services;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Enums;
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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateLeague([FromForm] CreateLeagueRequest request)
    {
        var userIdClaim = User.FindFirst("id")?.Value;

        if (!int.TryParse(userIdClaim, out var organizerId))
        {
            throw new UnauthorizedAccessException("User ID could not be found in the token.");
        }

        LeagueCreationResult result = await _leagueService.CreateLeague(request, organizerId);



        return Ok(new ApiResponse<LeagueResponse>(
            result.UploadWarning == null ? "success" : "warning",
            result.UploadWarning ?? "League created successfully",
            result.League.ToResponse()
        ));

    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeague([FromRoute] int id)
    {
        return Ok(await _leagueService.GetLeague(id));
    }


    [HttpPost("{leagueId}/join")]
    public async Task<IActionResult> JoinLeague(int leagueId, [FromBody] JoinLeagueRequest request)
    {
        var userIdClaim = User.FindFirst("id")?.Value;

        if (!int.TryParse(userIdClaim, out var playerId))
        {
            throw new UnauthorizedAccessException("User ID could not be found in the token.");
        }

        var result = await _leagueService.JoinLeagueAsync(leagueId, playerId, request);

        return Ok(result);
    }

    [HttpPatch("~/api/league-entries/{entryId}")]
    [Authorize(Roles = "ADMIN, ORGANIZER")]
    public async Task<IActionResult> UpdateEntryStatus([FromRoute] int entryId, [FromQuery] LeagueTeamEntryStatus status)
    {
        var league_entry = await _leagueService.UpdateEntryStatus(entryId, status);
        return Ok(new ApiResponse<LeagueTeamEntry>("success", "The status has been updated successfully", league_entry));
    }
}