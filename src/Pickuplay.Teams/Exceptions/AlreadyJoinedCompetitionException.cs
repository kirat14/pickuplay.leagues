namespace Pickuplay.Teams.Exceptions;

public class AlreadyJoinedCompetitionException : DomainException
{
    public AlreadyJoinedCompetitionException(string message = "You have already joined this competition.") : base(message) { }
}