using System.ComponentModel.DataAnnotations;

using Pickuplay.Teams.Models;

namespace Pickuplay.DTOs;

public record CompetitionCreationResult([Required] Competition Competition, string? UploadWarning);