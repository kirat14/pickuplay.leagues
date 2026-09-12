using System.ComponentModel.DataAnnotations;

using Pickuplay.Teams.Models;

namespace Pickuplay.DTOs;

public record CompetitionCreationResult([Required] CompetitionResponse Competition, string? UploadWarning);