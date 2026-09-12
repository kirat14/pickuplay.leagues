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
[Route("api/competitions")]               // base route: all endpoints here start with /api/teams
public class CompetitionController : ControllerBase  // gives us Ok(), NotFound(), etc.
{
    public readonly ICompetitionService _competitionService;

    public CompetitionController(ICompetitionService competitionService)
    {
        _competitionService = competitionService;
    }

    [HttpPost]                     // maps HTTP POST requests to this method
    [Authorize(Roles = "ADMIN, ORGANIZER")]                    // requires a valid JWT token to access this endpoint
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateCompetition([FromForm] CreateCompetitionRequest request)
    {
        var userIdClaim = User.FindFirst("id")?.Value;

        if (!int.TryParse(userIdClaim, out var organizerId))
        {
            throw new UnauthorizedAccessException("User ID could not be found in the token.");
        }

        CompetitionCreationResult result = await _competitionService.CreateCompetition(request, organizerId);



        return Ok(new ApiResponse<CompetitionResponse>(
            result.UploadWarning == null ? "success" : "warning",
            result.UploadWarning ?? "Competition created successfully",
            result.Competition.ToResponse()
        ));

    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompetition([FromRoute] int id)
    {
        var competition = await _competitionService.GetCompetition(id);
        return Ok(new ApiResponse<CompetitionResponse>("success", "Competition retrived successfully.", competition));
    }


    [HttpPost("{competitionId}/join")]
    public async Task<IActionResult> JoinCompetition(int competitionId, [FromBody] JoinCompetitionRequest request)
    {
        var userIdClaim = User.FindFirst("id")?.Value;

        if (!int.TryParse(userIdClaim, out var playerId))
        {
            throw new UnauthorizedAccessException("User ID could not be found in the token.");
        }

        var result = await _competitionService.JoinCompetitionAsync(competitionId, playerId, request);

        return Ok(result);
    }

    [HttpPatch("~/api/competition-entries/{entryId}")]
    [Authorize(Roles = "ADMIN, ORGANIZER")]
    public async Task<IActionResult> UpdateEntry([FromRoute] int entryId, [FromBody] CompetitionTeamEntryRequest entry)
    {
        var competition_entry = await _competitionService.UpdateEntry(entryId, entry);
        return Ok(new ApiResponse<CompetitionTeamEntry>("success", "The competition entry has been updated successfully", competition_entry));
    }

    [HttpGet]
    public async Task<IActionResult> GetLeagues([FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var competitions = await _competitionService.GetCompetitions(page, pageSize);
        return Ok(new ApiResponse<PagedResponse<CompetitionResponse>>("success", "Leagues retrived successfully", competitions));
    }

    [HttpGet("~/api/competition-entries")]
    public async Task<IActionResult> GetPendingCompetitionEntries([FromQuery] CompetitionTeamEntryStatus status, int organizerId)
    {
        var competitionTeamEntries = await _competitionService.GetEntries(organizerId, status);
        return Ok(new ApiResponse<List<CompetitionTeamEntryResponse>>("success", "Entries has been retirived successfully", competitionTeamEntries));
    }
}