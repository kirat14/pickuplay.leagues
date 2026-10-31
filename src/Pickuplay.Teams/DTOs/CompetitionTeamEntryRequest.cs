using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.DTOs;

public record CompetitionTeamEntryRequest(
    int? TeamId,
    bool? IsTeam,
    int? GuestCount,
    string? Comment,
    CompetitionTeamEntryStatus? Status
);