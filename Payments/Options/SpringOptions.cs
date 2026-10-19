using System.ComponentModel.DataAnnotations;

namespace Pickuplay.Teams.Payments.Options;

public sealed class SpringOptions
{
    public const string SectionName = "Spring";

    [Required]
    public string BaseUrl { get; init; } = default!;

    [Required]
    public string ApiKey { get; init; } = default!;

    [Required]
    public string GameUserStatusPath { get; init; } = default!;
}
