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
[Route("api/teams")]               // base route: all endpoints here start with /api/teams
public class TeamController : ControllerBase  // gives us Ok(), NotFound(), etc.
{
    public readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeam([FromRoute] int id)
    {
        var team = await _teamService.GetTeam(id);
        return Ok(new ApiResponse<TeamResponse>("success", "Team retrived successfully.", team));
    }

    /*     [HttpGet]
        public async Task<IActionResult> GetLeagues([FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            var competitions = await _teamService.GetCompetitions(page, pageSize);
            return Ok(new ApiResponse<PagedResponse<CompetitionResponse>>("success", "Leagues retrived successfully", competitions));
        } */

}