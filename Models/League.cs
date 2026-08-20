using Pickuplay.Enums;

namespace Pickuplay.Teams.Models;

public class League
{
    public int Id { get; set; }

    public int OrganizerId { get; set; }

    public required string Name { get; set; }

    public long SportTypeId { get; set; }
    public SportType? SportType { get; set; }

    public required string City { get; set; }
    public required string Address { get; set; }

    public DateTime DateTime { get; set; }

    public string? Description { get; set; }

    public DateTime StartRegistration { get; set; }
    public DateTime EndRegistration { get; set; }

    public int NbrOfTeams { get; set; }
    public int MinTeamPlayers { get; set; }
    public int MaxTeamPlayers { get; set; }

    public LeagueFormat Format { get; set; }

    public decimal PricePlayer { get; set; }

    public TeamGender Gender { get; set; }

    public int MinimumAge { get; set; }

    public List<Team> Teams { get; set; } = new();
}