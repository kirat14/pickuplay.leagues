
using Pickuplay.DTOs;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Enums;
using Pickuplay.Teams.Models;

namespace Pickuplay.Services;

public interface ILeagueService
{
     Task<LeagueCreationResult> CreateLeague(CreateLeagueRequest request, int organizerId);

     Task<JoinLeagueResponse> JoinLeagueAsync(int leagueId, int playerId, JoinLeagueRequest request);

     Task<LeagueResponse> GetLeague(int id);
     Task<LeagueTeamEntry> UpdateEntryStatus(int entryId, LeagueTeamEntryStatus status);

}