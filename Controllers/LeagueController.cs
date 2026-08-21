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
    private readonly AppDbContext _context;
    private readonly IStorageService _storageService;

    public LeagueController(AppDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
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

        var league = new League
        {
            OrganizerId = organizerId,
            Name = request.Name,
            SportTypeId = request.SportTypeId,
            City = request.City,
            Address = request.Address,
            DateTime = request.DateTime,
            Description = request.Description,
            StartRegistration = request.StartRegistration,
            EndRegistration = request.EndRegistration,
            NbrOfTeams = request.NbrOfTeams,
            MinTeamPlayers = request.MinTeamPlayers,
            MaxTeamPlayers = request.MaxTeamPlayers,
            Format = request.Format,
            PricePlayer = request.PricePlayer,
            Gender = request.Gender,
            MinimumAge = request.MinimumAge
        };


        for (int i = 0; i < request.NbrOfTeams; i++)
        {
            var teamName = i < request.TeamNames.Count ? request.TeamNames[i] : $"Team {i + 1}";

            league.Teams.Add(new Team
            {
                Name = teamName
            });
        }

        _context.Leagues.Add(league);
        _context.SaveChanges();

        return Ok(new ApiResponse<LeagueResponse>("success", "League created successfully", new LeagueResponse(
            league.Id,
            league.Name,
            league.City,
            league.DateTime,
            league.Teams.Select(t => t.Name).ToList()
        )));
    }
}