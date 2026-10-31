namespace Pickuplay.Teams.DTOs;

public class PagedResponse<T>
{
    public List<T> Content { get; set; } = [];

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalElements { get; set; }

    public bool HasNext { get; set; }

    public bool HasPrevious { get; set; }
}