namespace Demo.Infrastructure.ResultModel
{
    public class LoginResult : BaseResult
    {
        public string? UserId { get; set; }

        public string? Token { get; set; }

        public DateTime TokenExpireTime { get; set; }
    }
}
