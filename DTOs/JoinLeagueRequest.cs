namespace Pickuplay.Teams.DTOs;

public record JoinLeagueRequest(int LeagueId, int TeamId, string? Comment, bool IsTeam, int GuestCount = 0);