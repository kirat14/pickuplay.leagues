namespace Pickuplay.Teams.Exceptions;

public class LeagueRegistrationPeriodException : DomainException
{
    public LeagueRegistrationPeriodException(string message = "Registration for this league is currently closed.") : base(message) { }
}