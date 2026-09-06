using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Models;

public class LeagueTeamEntry
{
    public int Id { get; set; }

    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public int LeagueId { get; set; }
    public League League { get; set; } = null!;

    public int PlayerId { get; set; }

    public bool IsTeam { get; set; } = false;      // true = paid for entire team slot
    public int GuestCount { get; set; } = 0;        // guests this player is paying for

    public string? Comment { get; set; }

    public LeagueTeamEntryStatus Status { get; set; } = LeagueTeamEntryStatus.Pending;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}