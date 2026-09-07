using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Models;

public class CompetitionTeamEntry
{
    public int Id { get; set; }

    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public int CompetitionId { get; set; }
    public Competition Competition { get; set; } = null!;

    public int PlayerId { get; set; }

    public bool IsTeam { get; set; } = false;      // true = paid for entire team slot
    public int GuestCount { get; set; } = 0;        // guests this player is paying for

    public string? Comment { get; set; }

    public CompetitionTeamEntryStatus Status { get; set; } = CompetitionTeamEntryStatus.Pending;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}