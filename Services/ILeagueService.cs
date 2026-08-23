
using Pickuplay.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Services;

public interface ILeagueService
{
     Task<LeagueCreationResult> CreateLeague(CreateLeagueRequest request, int organizerId);
}