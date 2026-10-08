using Pickuplay.Enums;

namespace Pickuplay.DTOs;

public record ApiResponse<T>(ApiResponseStatus type, string message, T data) { }