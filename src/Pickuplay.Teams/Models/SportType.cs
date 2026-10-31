namespace Pickuplay.Teams.Models;

public class SportType
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}