namespace Pickuplay.Teams.Exceptions;

public class TeamNotFoundException : DomainException
{
    public TeamNotFoundException(string message = "Team not found in this league.") : base(message) { }
}