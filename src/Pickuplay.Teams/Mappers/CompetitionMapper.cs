using Pickuplay.DTOs;
using Pickuplay.Services;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Mappers;

public static class CompetitionMapper
{
    public static CompetitionResponse ToResponse(this Competition competition, IStorageService storageService)
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
            Teams: competition.Teams.Select(t => new TeamResponse(
                t.Id,
                t.CompetitionId,
                t.Name,
                t.CaptainId,
                t.Logo,
                t.Color,
                t.Wins,
                t.Losses,
                t.Points,
                t.Entries.Count + t.Entries.Sum(e => e.GuestCount)
            )).ToList(),
            TeamCount: competition.Teams.Count,
            AvailableSpots: ((competition.TeamSize + competition.NbrOfSubs) * competition.NbrOfTeams) - (competition.Entries.Count + competition.Entries.Sum(e => e.GuestCount)),
            storageService.GetFileUrl(competition.Logo),
            storageService.GetFileUrl(competition.CoverPhoto)
        );
    }
}