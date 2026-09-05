using Pickuplay.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Mappers;

public static class LeagueMapper
{
    public static LeagueResponse ToResponse(this League league)
    {
        return new LeagueResponse(
            Id: league.Id,
            OrganizerId: league.OrganizerId,
            Name: league.Name,
            SportType: league.SportType.Name,
            City: league.City,
            Address: league.Address,
            StartDate: league.StartDate,
            Description: league.Description,
            StartRegistration: league.StartRegistration,
            EndRegistration: league.EndRegistration,
            NbrOfTeams: league.NbrOfTeams,
            TeamSize: league.TeamSize,
            NbrOfSubs: league.NbrOfSubs,
            Format: league.Format.ToString(),
            PricePlayer: league.PricePlayer,
            Gender: league.Gender.ToString(),
            MinimumAge: league.MinimumAge,
            Comment: league.Comment,
            Referee: league.Referee,
            Prize: league.Prize,
            Pennies: league.Pennies,
            Teams: league.Teams.ToDictionary(
                team => team.Id,
                team => team.Name
            )
        );
    }
}