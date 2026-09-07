namespace Pickuplay.Teams.Exceptions;

public class CompetitionRegistrationPeriodException : DomainException
{
    public CompetitionRegistrationPeriodException(string message = "Registration for this competition is currently closed.") : base(message) { }
}