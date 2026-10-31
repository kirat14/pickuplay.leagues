using Pickuplay.Enums;

namespace Pickuplay.Teams.Models;

public class Competition
{
    public int Id { get; set; }

    public int OrganizerId { get; set; }

    public required string Name { get; set; }

    public long SportTypeId { get; set; }
    public SportType SportType { get; set; } = null!;

    public required string City { get; set; }
    public required string Address { get; set; }

    public DateTime StartDate { get; set; }

    public string? Description { get; set; }

    public DateTime StartRegistration { get; set; }
    public DateTime EndRegistration { get; set; }

    public int NbrOfTeams { get; set; }
    public int TeamSize { get; set; }
    public int NbrOfSubs { get; set; }

    public CompetitionFormat Format { get; set; }

    public decimal PricePlayer { get; set; }

    public TeamGender Gender { get; set; }

    public int? MinimumAge { get; set; }

    public string? Comment { get; set; }

    public string? Logo { get; set; }
    public string? CoverPhoto { get; set; }

    private readonly List<Team> _teams = new();
    public IReadOnlyList<Team> Teams => _teams; // shortcut for get { return _teams; }

    public IList<CompetitionTeamEntry> Entries { get; set; } = [];


    public bool Referee { get; set; }
    public bool Prize { get; set; }
    public bool Pennies { get; set; }

    public void AddTeam(Team team)
    {
        if (_teams.Count >= NbrOfTeams)
            throw new InvalidOperationException("Competition Team Size is " + NbrOfTeams);

        _teams.Add(team);
    }
}