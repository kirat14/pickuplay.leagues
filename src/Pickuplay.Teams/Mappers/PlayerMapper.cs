using Pickuplay.DTOs;
using Pickuplay.Services;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Mappers;

public static class PlayerMapper
{
    public static PlayerResponse ToResponse(this Player p, int guestCount)
    {
        return new PlayerResponse(
                p.Id,
                p.FirstName,
                p.LastName,
                p.SkillLevel,
                guestCount
            );
    }
}