using System.ComponentModel.DataAnnotations;

using Pickuplay.Enums;

namespace Pickuplay.Teams.DTOs;

public record UpdateCompetition
(
    string? Name,
    long? SportTypeId,
    string? City,
    string? Address,
    DateTime? StartDate,
    string? Description,
    DateTime? StartRegistration,
    DateTime? EndRegistration,
    [Range(1, int.MaxValue, ErrorMessage = "Number of teams must be at least 1.")]
    int? NbrOfTeams,
    List<string>? TeamNames,
    int? TeamSize,
    int? NbrOfSubs,
    CompetitionFormat? Format,
    decimal? PricePlayer,
    TeamGender? Gender,
    int? MinimumAge,
    string? Comment,
    IFormFile? Logo,
    IFormFile? CoverPhoto,
    bool? Referee,
    bool? Prize,
    bool? Pennies
)
{ }