using Microsoft.EntityFrameworkCore;

using Pickuplay.DTOs;
using Pickuplay.Mappers;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Exceptions;
using Pickuplay.Teams.Models;

namespace Pickuplay.Services;

class CompetitionService : ICompetitionService
{
    private readonly AppDbContext _context;
    private readonly IStorageService _storageService;

    public CompetitionService(AppDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<CompetitionCreationResult> CreateCompetition(CreateCompetitionRequest request, int organizerId)
    {

        var competition = new Competition
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

            competition.Teams.Add(new Team
            {
                Name = teamName
            });
        }

        _context.Competitions.Add(competition);
        _context.SaveChanges();

        competition = await _context.Competitions
        .FirstAsync(l => l.Id == competition.Id);

        string? uploadWarning = null;

        try
        {
            var logoTask = request.Logo != null
            ? _storageService.SaveFile(request.Logo, "competitions")
            : Task.FromResult<string?>(null);

            var coverTask = request.CoverPhoto != null
                ? _storageService.SaveFile(request.CoverPhoto, "competitions")
                : Task.FromResult<string?>(null);

            string?[] results = await Task.WhenAll(logoTask, coverTask);

            competition.Logo = results[0];
            competition.CoverPhoto = results[1];
            _context.SaveChanges();
        }
        catch (System.Exception)
        {

            uploadWarning = "Competition was created, but the image upload failed. You can try uploading it again later.";
        }

        return new CompetitionCreationResult(competition.ToResponse(_storageService), uploadWarning);
    }

    public async Task<CompetitionResponse> GetCompetition(int id)
    {
        var competition = await _context.Competitions
        .Include(c => c.Teams)
        .Include(c => c.Entries)
        .ThenInclude(e => e.Player)
        .FirstOrDefaultAsync(l => l.Id == id);

        if (competition == null)
            throw new CompetitionNotFoundException();

        return competition.ToResponse(_storageService);
    }

    public async Task<PagedResponse<CompetitionResponse>> GetCompetitions(int page, int pageSize)
    {
        var totalElements = await _context.Competitions.CountAsync();

        var leagues = await _context.Competitions
        .Include(c => c.Teams)
        .Include(c => c.Entries)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalElements / (double)pageSize);

        return new PagedResponse<CompetitionResponse>
        {
            Content = leagues.Select(l => l.ToResponse(_storageService)).ToList(),
            CurrentPage = page,
            TotalPages = totalPages,
            TotalElements = totalElements,
            HasNext = page < totalPages,
            HasPrevious = page > 1
        };

    }

    public bool isCompetitionFull(Competition competition, int totalPlayers)
    {
        int occupiedSlots = 0;

        foreach (var entry in competition.Entries)
        {
            if (entry.Status is not (CompetitionTeamEntryStatus.Rejected or CompetitionTeamEntryStatus.Cancelled))
            {
                occupiedSlots += entry.IsTeam ? competition.TeamSize + competition.NbrOfSubs : 1 + entry.GuestCount;
                if (occupiedSlots >= totalPlayers)
                    return true;
            }
        }

        return false;
    }

    public bool IsTeamFull(Team team, int maxTeamSize)
    {
        int occupiedSlots = 0;

        foreach (var entry in team.Entries)
        {
            if (entry.IsTeam)
                return true;

            if (entry.Status is not (CompetitionTeamEntryStatus.Rejected or CompetitionTeamEntryStatus.Cancelled))
            {
                occupiedSlots += 1 + entry.GuestCount;
                if (occupiedSlots >= maxTeamSize)
                    return true;
            }
        }

        return false;
    }

    public async Task<JoinCompetitionResponse> JoinCompetitionAsync(int competitionId, int playerId, JoinCompetitionRequest request)
    {
        var competition = await _context.Competitions
        .Include(c => c.Entries)
        .Include(c => c.Teams)
        .FirstOrDefaultAsync(c => c.Id == request.CompetitionId)
        ?? throw new CompetitionNotFoundException($"Competition with ID {request.CompetitionId} was not found.");

        DateTime now = DateTime.Now;
        if (now > competition.EndRegistration || now < competition.StartRegistration)
            throw new CompetitionRegistrationPeriodException();

        int maxTeamSize = competition.TeamSize + competition.NbrOfSubs;

        if (request.TeamId > 0 && !competition.Teams.Any(t => t.Id == request.TeamId))
            throw new TeamNotFoundException();
        else
        {
            if (request.TeamId > 0)
            {
                if (IsTeamFull(competition.Teams.First(t => t.Id == request.TeamId), maxTeamSize))
                    throw new TeamFullException();
            }
            else
            {
                if (isCompetitionFull(competition, maxTeamSize * competition.NbrOfTeams))
                    throw new CompetitionFullException();
            }
        }

        var entry = competition.Entries.FirstOrDefault(e =>
            e.PlayerId == playerId &&
            e.Status is not (CompetitionTeamEntryStatus.Pending or CompetitionTeamEntryStatus.Confirmed));

        if (entry != null)
        {
            entry.Status = CompetitionTeamEntryStatus.Pending;
            entry.JoinedAt = DateTime.UtcNow;
        }
        else
        {
            entry = new CompetitionTeamEntry
            {
                TeamId = request.TeamId > 0 ? request.TeamId : null,
                CompetitionId = competitionId,
                PlayerId = playerId,
                IsTeam = request.IsTeam,
                GuestCount = request.GuestCount,
                Comment = request.Comment,
                Status = CompetitionTeamEntryStatus.Pending
            };
            _context.CompetitionTeamEntries.Add(entry);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Duplicate entry") == true)
        {
            throw new AlreadyJoinedCompetitionException();
        }

        return new JoinCompetitionResponse(
            entry.Id,
            entry.Team?.Name,
            entry.IsTeam,
            entry.GuestCount,
            entry.Comment,
            entry.Status.ToString(),
            entry.JoinedAt);
    }

    public async Task<CompetitionTeamEntry> UpdateEntry(int entryId, CompetitionTeamEntryRequest entry)
    {
        var competition_entry = await _context.CompetitionTeamEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (competition_entry == null)
        {
            throw new CompetitionTeamEntryNotFoundException();
        }

        competition_entry.Status = entry.Status ?? competition_entry.Status;
        competition_entry.TeamId = entry.TeamId > 0 ? entry.TeamId : competition_entry.TeamId;
        competition_entry.Comment = entry.Comment ?? competition_entry.Comment;
        competition_entry.IsTeam = entry.IsTeam ?? competition_entry.IsTeam;
        competition_entry.GuestCount = entry.GuestCount > 0 ? entry.GuestCount.Value : competition_entry.GuestCount;

        await _context.SaveChangesAsync();

        return competition_entry;
    }

    public async Task<List<CompetitionTeamEntryResponse>> GetEntries(int organizerId, CompetitionTeamEntryStatus entryStatus)
    {

        var competitionTeamEntries = await _context.CompetitionTeamEntries
        .Include(e => e.Player)
        .Include(e => e.Team)
        .Where(e => e.Competition.OrganizerId == organizerId && e.Status == entryStatus)
        .ToListAsync();

        return competitionTeamEntries.Select(e => e.ToResponse()).ToList();
    }
}