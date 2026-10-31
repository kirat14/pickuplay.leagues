namespace Pickuplay.Teams.DTOs;

public record JoinCompetitionRequest(int TeamId, string? Comment, bool IsTeam, int GuestCount = 0);