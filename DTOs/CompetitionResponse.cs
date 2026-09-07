namespace Pickuplay.DTOs;

public record CompetitionResponse(
    int Id,
    int OrganizerId,
    string Name,
    string SportType,
    string City,
    string Address,
    DateTime StartDate,
    string? Description,
    DateTime StartRegistration,
    DateTime EndRegistration,
    int NbrOfTeams,
    int TeamSize,
    int NbrOfSubs,
    string Format,
    decimal PricePlayer,
    string Gender,
    int? MinimumAge,
    string? Comment,
    bool Referee,
    bool Prize,
    bool Pennies,
    IDictionary<int, string> Teams
);