using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.DTOs;

public record CompetitionTeamEntryResponse(
    int Id,
    int TeamId,
    int CompetitionId,
    Player Player,
    bool IsTeam,
    int GuestCount,
    string? Comment,
    CompetitionTeamEntryStatus Status,
    DateTime JoinedAt
);