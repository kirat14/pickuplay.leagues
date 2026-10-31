namespace Pickuplay.Teams.Exceptions;

public class TeamFullException : DomainException
{
    public TeamFullException(string message = "Not enough space left on this team.") : base(message) { }
}