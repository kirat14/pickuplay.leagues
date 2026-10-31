using Pickuplay.DTOs;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Mappers;

public static class CompetitionEntryMapper
{
    public static CompetitionTeamEntryResponse ToResponse(this CompetitionTeamEntry competitionEntry)
    {
        return new CompetitionTeamEntryResponse(
            competitionEntry.Id,
            competitionEntry.TeamId,
            competitionEntry.Team?.Name,
            competitionEntry.CompetitionId,
            competitionEntry.Player,
            competitionEntry.IsTeam,
            competitionEntry.GuestCount,
            competitionEntry.Comment,
            competitionEntry.Status,
            competitionEntry.JoinedAt
        );
    }
}