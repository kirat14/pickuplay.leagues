namespace Pickuplay.Teams.Exceptions;

public class LeagueNotFoundException : DomainException
{
    public LeagueNotFoundException(string message = "League not found.") : base(message) { }
}