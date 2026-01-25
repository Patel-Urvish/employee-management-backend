namespace EmployeeManagement.Dtos;

public class PaginationDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = String.Empty;
    public string SortDirection { get; set; } = "asc";
}

