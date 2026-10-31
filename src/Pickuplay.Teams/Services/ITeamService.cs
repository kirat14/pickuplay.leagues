
using Pickuplay.Teams.DTOs;

namespace Pickuplay.Services;

public interface ITeamService
{
     Task<TeamResponse> GetTeam(int id);
     Task<TeamResponse> GetTeams(int competitionId);

}