
using Pickuplay.DTOs;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Models;

namespace Pickuplay.Services;

public interface ICompetitionService
{
     Task<CompetitionCreationResult> CreateCompetition(CreateCompetitionRequest request, int organizerId);

     Task<JoinCompetitionResponse> JoinCompetitionAsync(int competitionId, int playerId, JoinCompetitionRequest request);

     Task<CompetitionResponse> GetCompetition(int id);
     Task<CompetitionTeamEntry> UpdateEntryStatus(int entryId, CompetitionTeamEntryStatus status);

}