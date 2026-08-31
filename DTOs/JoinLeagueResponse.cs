namespace Pickuplay.Teams.DTOs;

public record JoinLeagueResponse(int Id, String TeamName, bool IsTeam, int GuestCount, String Status, DateTime JoinedAt);