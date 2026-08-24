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

    public async Task<LeagueCreationResult> CreateLeague(CreateLeagueRequest request, int organizerId)
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
            TeamSize = request.TeamSize,
            NbrOfSubs = request.NbrOfSubs,
            Format = request.Format,
            PricePlayer = request.PricePlayer,
            Gender = request.Gender,
            MinimumAge = request.MinimumAge,
            Comment = request.Comment,
            Referee = request.Referee,
            Prize = request.Prize,
            Pennies = request.Pennies
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

        string? uploadWarning = null;

        try
        {
            var logoTask = request.Logo != null
            ? _storageService.SaveFile(request.Logo, $"logo_{league.Id}", "leagues")
            : Task.FromResult<string?>(null);

            var coverTask = request.CoverPhoto != null
                ? _storageService.SaveFile(request.CoverPhoto, $"cover_{league.Id}", "leagues")
                : Task.FromResult<string?>(null);

            await Task.WhenAll(logoTask, coverTask);

            league.Logo = await logoTask;
            league.CoverPhoto = await coverTask;
            _context.SaveChanges();
        }
        catch (System.Exception)
        {

            uploadWarning = "League was created, but the image upload failed. You can try uploading it again later.";
        }

        return new LeagueCreationResult(league, uploadWarning);
    }
}