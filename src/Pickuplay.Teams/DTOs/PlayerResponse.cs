using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.DTOs;

public record PlayerResponse(
    long Id,
    string FirstName,
    string LastName,
    string SkillLevel,
    int guestCount
);