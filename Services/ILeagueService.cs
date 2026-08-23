
using Pickuplay.DTOs;
using Pickuplay.Teams.Models;

namespace Pickuplay.Services;

public interface ILeagueService
{
    Task<League> CreateLeague(CreateLeagueRequest request, int organizerId);
}