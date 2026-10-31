using Pickuplay.DTOs;
using Pickuplay.Services;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Mappers;

public static class TeamMapper
{
    public static TeamResponse ToResponse(this Team t)
    {
        return new TeamResponse(
                t.Id,
                t.CompetitionId,
                t.Name,
                t.CaptainId,
                t.Logo,
                t.Color,
                t.Wins,
                t.Losses,
                t.Points,
                t.Entries.Count + t.Entries.Sum(e => e.GuestCount),
                t.Entries.Select(e => e.Player.ToResponse(e.GuestCount)).ToList()
            );
    }
}