namespace Pickuplay.Teams.DTOs;

public record JoinCompetitionResponse(int Id, string TeamName, bool IsTeam, int GuestCount, string? Comment, string Status, DateTime JoinedAt);