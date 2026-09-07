using Microsoft.EntityFrameworkCore;

using Pickuplay.DTOs;
using Pickuplay.Mappers;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Exceptions;
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
            StartDate = request.StartDate,
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

        league = await _context.Leagues
        .FirstAsync(l => l.Id == league.Id);

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

    public async Task<LeagueResponse> GetLeague(int id)
    {
        var league = await _context.Leagues
        .Include(l => l.Teams)
        .FirstOrDefaultAsync(l => l.Id == id);

        if (league == null)
            throw new LeagueNotFoundException();

        return league.ToResponse();
    }

    public async Task<JoinLeagueResponse> JoinLeagueAsync(int leagueId, int playerId, JoinLeagueRequest request)
    {
        var team = await _context.Teams
            .Include(t => t.Entries)
            .Include(t => t.League)
            .FirstOrDefaultAsync(t => t.Id == request.TeamId && t.LeagueId == leagueId);

        if (team == null)
            throw new TeamNotFoundException();

        DateTime now = DateTime.Now;
        if (now > team.League.EndRegistration || now < team.League.StartRegistration)
            throw new LeagueRegistrationPeriodException();

        var occupiedSlots = team.Entries.Sum(e => e.IsTeam ? team.League.TeamSize : 1 + e.GuestCount);
        var requestedSlots = request.IsTeam ? team.League.TeamSize : 1 + request.GuestCount;

        if (occupiedSlots + requestedSlots > team.League.TeamSize)
            throw new TeamFullException();

        var entry = new LeagueTeamEntry
        {
            TeamId = team.Id,
            LeagueId = leagueId,
            PlayerId = playerId,
            IsTeam = request.IsTeam,
            GuestCount = request.GuestCount,
            Comment = request.Comment,
            Status = LeagueTeamEntryStatus.Pending
        };

        try
        {
            _context.LeagueTeamEntries.Add(entry);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Duplicate entry") == true)
        {
            throw new AlreadyJoinedLeagueException();
        }

        return new JoinLeagueResponse(
            entry.Id,
            entry.Team.Name,
            entry.IsTeam,
            entry.GuestCount,
            entry.Comment,
            entry.Status.ToString(),
            entry.JoinedAt);
    }

    public async Task<LeagueTeamEntry> UpdateEntryStatus(int entryId, LeagueTeamEntryStatus status)
    {
        var league_entry = await _context.LeagueTeamEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (league_entry == null)
        {
            throw new LeagueTeamEntryNotFoundException();
        }

        league_entry.Status = status;
        await _context.SaveChangesAsync();

        return league_entry;
    }
}