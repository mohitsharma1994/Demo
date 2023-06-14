namespace Demo.Infrastructure.RequestModel
{
    public class GetUserListRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Sort { get; set; }
        public string? SortDirection { get; set; } 
        public string? Search { get; set; } 
    }
}
