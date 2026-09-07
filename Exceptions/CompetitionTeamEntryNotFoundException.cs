namespace Pickuplay.Teams.Exceptions;

public class CompetitionTeamEntryNotFoundException : DomainException
{
    public CompetitionTeamEntryNotFoundException(string message = "The resource you are requesting is not found.") : base(message) { }
}