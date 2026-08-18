using System.Data;
using Pickuplay.Enums;

namespace Pickuplay.Teams.Models;

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public long SportTypeId { get; set; }
    public SportType? SportType { get; set; }

    public TeamGender Gender { get; set; }
    public TeamAgeGroup AgeGroup { get; set; }
    public TeamSize TeamSize { get; set; }
    
    public required string City { get; set; }

    // Optional Fields
    public string? Logo { get; set; }
    public string? Description { get; set; }

    // Auto set when team is created
    public int TeamLeaderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


}
