namespace Pickuplay.Teams.DTOs;

public record JoinLeagueResponse(int Id, string TeamName, bool IsTeam, int GuestCount, string? Comment, string Status, DateTime JoinedAt);