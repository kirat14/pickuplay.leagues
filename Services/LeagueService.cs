using Microsoft.EntityFrameworkCore;
using Pickuplay.DTOs;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.Models;

namespace Pickuplay.Services;

class LeagueService : ILeagueService
{
    private readonly AppDbContext _context;
    private readonly IStorageService _storageService;

    public LeagueService(AppDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public League CreateLeague(CreateLeagueRequest request, int organizerId)
    {

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
            MinimumAge = request.MinimumAge,
            Comment = request.Comment
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

        if (request.Logo != null)
        {
            var extension = _storageService.SaveFile(request.Logo, $"logo_{league.Id}", "leagues");
            league.Logo = extension;
            _context.SaveChanges();
        }

        if (request.CoverPhoto != null)
        {
            var extension = _storageService.SaveFile(request.CoverPhoto, $"cover_{league.Id}", "leagues");
            league.CoverPhoto = extension;
            _context.SaveChanges();
        }

        return league;
    }
}