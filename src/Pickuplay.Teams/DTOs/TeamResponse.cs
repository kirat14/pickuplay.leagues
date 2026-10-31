using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.DTOs;

public record TeamResponse(
    int Id,
    int CompetitionId,
    string Name,
    int? CaptainId,
    string? Logo,
    string? Color,
    int Wins,
    int Losses,
    int Points,
    int JoinedPlayersCount,
    IList<PlayerResponse>? Players = null
);