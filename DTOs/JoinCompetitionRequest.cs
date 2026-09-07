namespace Pickuplay.Teams.DTOs;

public record JoinCompetitionRequest(int CompetitionId, int TeamId, string? Comment, bool IsTeam, int GuestCount = 0);