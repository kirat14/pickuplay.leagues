using System.ComponentModel.DataAnnotations;
using Pickuplay.Teams.Models;

namespace Pickuplay.DTOs;

public record LeagueCreationResult([Required] League League, string? UploadWarning);