namespace Pickuplay.Teams.Exceptions;

public class LeagueTeamEntryNotFoundException : DomainException
{
    public LeagueTeamEntryNotFoundException(string message = "The resource you are requesting is not found.") : base(message) { }
}