namespace Pickuplay.Teams.Exceptions;

public class CompetitionFullException : DomainException
{
    public CompetitionFullException(string message = "Competition is Full.") : base(message) { }
}