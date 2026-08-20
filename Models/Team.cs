namespace Pickuplay.Teams.Models;

public class Team
{
    public int Id { get; set; }

    public int LeagueId { get; set; }
    public League? League { get; set; }

    public required string Name { get; set; }

    public int? CaptainId { get; set; }

    public string? Logo { get; set; }

    public string? Color { get; set; }

    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Points { get; set; }
}