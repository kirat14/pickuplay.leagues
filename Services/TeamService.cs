
using Microsoft.EntityFrameworkCore;

using Pickuplay.Mappers;
using Pickuplay.Services;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.DTOs;
using Pickuplay.Teams.Exceptions;
using Pickuplay.Teams.Models;

namespace Pickuplay.Services;

public class TeamService : ITeamService
{
    private readonly AppDbContext _context;

    public TeamService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeamResponse> GetTeam(int id)
    {
        var team = await _context.Teams
        .Include(t => t.Entries)
        .ThenInclude(e => e.Player)
        .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
            throw new TeamNotFoundException();

        return team.ToResponse();
    }

    public Task<TeamResponse> GetTeams(int competitionId)
    {
        throw new NotImplementedException();
    }
}