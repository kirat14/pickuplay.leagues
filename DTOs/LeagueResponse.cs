namespace Pickuplay.DTOs;

public record LeagueResponse(
    int Id,
    string Name,
    string City,
    DateTime DateTime,
    List<string> TeamNames
);