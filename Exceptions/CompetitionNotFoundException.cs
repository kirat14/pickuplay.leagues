namespace Pickuplay.Teams.Exceptions;

public class CompetitionNotFoundException : DomainException
{
    public CompetitionNotFoundException(string message = "Competition not found.") : base(message) { }
}