namespace Demo.Infrastructure.ResultModel
{
    public class UserResult
    {
        public PaginatedMetaData? MetaData { get; set; }
        public List<UserResultItem>? Items { get; set; }
    }

    public class UserResultItem
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
