namespace Demo.Infrastructure.ResultModel
{
    public class PaginatedMetaData
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public decimal TotalPages => Math.Ceiling(TotalItems * 1M / ItemsPerPage * 1M);
    }
}
