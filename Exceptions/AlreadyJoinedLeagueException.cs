namespace Pickuplay.Teams.Exceptions;

public class AlreadyJoinedLeagueException : DomainException
{
    public AlreadyJoinedLeagueException(string message = "You have already joined this league.") : base(message) { }
}