namespace Demo.Infrastructure.ResultModel
{
    public class UserLogResult
    {
        public PaginatedMetaData? MetaData { get; set; }
        public List<UserLogResultItem>? Items { get; set; }
    }

    public class UserLogResultItem
    {
        public bool IsSuccess { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
    }
}
