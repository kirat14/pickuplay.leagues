namespace Pickuplay.DTOs;
public record ApiResponse<T>(string type, string message, T data){}