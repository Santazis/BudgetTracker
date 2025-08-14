namespace BudgetTracker.Domain.Common.Pagination;

public class PaginationRequest
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = DefaultPageSize;

    public PaginationRequest() {}
    public PaginationRequest(int pageNumber, int pageSize)
    {
        pageNumber = pageNumber  <=  0 ? 1 : pageNumber;
        pageSize = pageSize is < 0 or > MaxPageSize ? DefaultPageSize : pageSize;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    public int Skip => (PageNumber - 1) * PageSize;
}
