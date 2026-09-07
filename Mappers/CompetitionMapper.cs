using Pickuplay.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Mappers;

public static class CompetitionMapper
{
    public static CompetitionResponse ToResponse(this Competition competition)
    {
        return new CompetitionResponse(
            Id: competition.Id,
            OrganizerId: competition.OrganizerId,
            Name: competition.Name,
            SportType: competition.SportType.Name,
            City: competition.City,
            Address: competition.Address,
            StartDate: competition.StartDate,
            Description: competition.Description,
            StartRegistration: competition.StartRegistration,
            EndRegistration: competition.EndRegistration,
            NbrOfTeams: competition.NbrOfTeams,
            TeamSize: competition.TeamSize,
            NbrOfSubs: competition.NbrOfSubs,
            Format: competition.Format.ToString(),
            PricePlayer: competition.PricePlayer,
            Gender: competition.Gender.ToString(),
            MinimumAge: competition.MinimumAge,
            Comment: competition.Comment,
            Referee: competition.Referee,
            Prize: competition.Prize,
            Pennies: competition.Pennies,
            Teams: competition.Teams.ToDictionary(
                team => team.Id,
                team => team.Name
            ),
            TeamCount: competition.Teams.Count
        );
    }
}