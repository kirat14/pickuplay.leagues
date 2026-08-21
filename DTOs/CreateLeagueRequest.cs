using System.ComponentModel.DataAnnotations;
using Pickuplay.Enums;

namespace Pickuplay.DTOs;

public class CreateLeagueRequest
{
    public required string Name { get; set; }
    public required long SportTypeId { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
    public required DateTime DateTime { get; set; }
    public string? Description { get; set; }
    public required DateTime StartRegistration { get; set; }
    public required DateTime EndRegistration { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Number of teams must be at least 1.")]
    public required int NbrOfTeams { get; set; }
    public required List<string> TeamNames {get; set;}
    public int MinTeamPlayers { get; set; }
    public int MaxTeamPlayers { get; set; }
    public LeagueFormat Format { get; set; }
    public decimal PricePlayer { get; set; }
    public TeamGender Gender { get; set; }
    public int MinimumAge { get; set; }
}