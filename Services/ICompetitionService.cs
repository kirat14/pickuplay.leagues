
using Pickuplay.DTOs;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Enums;

namespace Pickuplay.Services;

public interface ICompetitionService
{
     Task<CompetitionCreationResult> CreateCompetition(CreateCompetitionRequest request, int organizerId);

     Task<JoinCompetitionResponse> JoinCompetitionAsync(int competitionId, int playerId, JoinCompetitionRequest request);

     Task<CompetitionResponse> GetCompetition(int id);
     Task<CompetitionTeamEntry> UpdateEntry(int entryId, CompetitionTeamEntryRequest entry);

     Task<PagedResponse<CompetitionResponse>> GetCompetitions(int page, int pageSize);
     Task<List<CompetitionTeamEntryResponse>> GetEntries(int organizerId, CompetitionTeamEntryStatus entryStatus);


}