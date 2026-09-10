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
            ? _storageService.SaveFile(request.Logo, $"logo_{competition.Id}", "competitions")
            : Task.FromResult<string?>(null);

            var coverTask = request.CoverPhoto != null
                ? _storageService.SaveFile(request.CoverPhoto, $"cover_{competition.Id}", "competitions")
                : Task.FromResult<string?>(null);

            await Task.WhenAll(logoTask, coverTask);

            competition.Logo = await logoTask;
            competition.CoverPhoto = await coverTask;
            _context.SaveChanges();
        }
        catch (System.Exception)
        {

            uploadWarning = "Competition was created, but the image upload failed. You can try uploading it again later.";
        }

        return new CompetitionCreationResult(competition, uploadWarning);
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

        return competition.ToResponse();
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
            Content = leagues.Select(l => l.ToResponse()).ToList(),
            CurrentPage = page,
            TotalPages = totalPages,
            TotalElements = totalElements,
            HasNext = page < totalPages,
            HasPrevious = page > 1
        };

    }

    public async Task<JoinCompetitionResponse> JoinCompetitionAsync(int competitionId, int playerId, JoinCompetitionRequest request)
    {
        var team = await _context.Teams
            .Include(t => t.Entries)
            .Include(t => t.Competition)
            .FirstOrDefaultAsync(t => t.Id == request.TeamId && t.CompetitionId == competitionId);

        if (team == null)
            throw new TeamNotFoundException();

        DateTime now = DateTime.Now;
        if (now > team.Competition.EndRegistration || now < team.Competition.StartRegistration)
            throw new CompetitionRegistrationPeriodException();

        var occupiedSlots = team.Entries
        .Where(e => e.Status is not (CompetitionTeamEntryStatus.Rejected or CompetitionTeamEntryStatus.Cancelled))
        .Sum(e => e.IsTeam ? team.Competition.TeamSize : 1 + e.GuestCount);
        var requestedSlots = request.IsTeam ? team.Competition.TeamSize : 1 + request.GuestCount;

        if (occupiedSlots + requestedSlots > team.Competition.TeamSize)
            throw new TeamFullException();


        var entry = team.Entries.FirstOrDefault(e =>
            e.PlayerId == playerId &&
            e.Status is not (CompetitionTeamEntryStatus.Pending or CompetitionTeamEntryStatus.Confirmed));

        if (entry != null)
        {
            entry.Status = CompetitionTeamEntryStatus.Pending;
        }
        else
        {
            entry = new CompetitionTeamEntry
            {
                TeamId = team.Id,
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
            entry.Team.Name,
            entry.IsTeam,
            entry.GuestCount,
            entry.Comment,
            entry.Status.ToString(),
            entry.JoinedAt);
    }

    public async Task<CompetitionTeamEntry> UpdateEntryStatus(int entryId, CompetitionTeamEntryStatus status)
    {
        var competition_entry = await _context.CompetitionTeamEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (competition_entry == null)
        {
            throw new CompetitionTeamEntryNotFoundException();
        }

        competition_entry.Status = status;
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