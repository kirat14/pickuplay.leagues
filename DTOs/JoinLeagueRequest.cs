namespace Pickuplay.Teams.DTOs;

public record JoinLeagueRequest(int LeagueId, int TeamId, bool IsTeam, int GuestCount = 0);