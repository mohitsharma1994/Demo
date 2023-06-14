using Demo.Infrastructure.Entity;
using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.ResultModel;

namespace Demo.BLL.IService
{
    public interface IUserService
    {
        Task<UserResult> GetUserListAsync(GetUserListRequest request);

        Task<bool> AddUserLoginLogAsync(bool isSuccess,string userId);

        Task<bool> AddUserLogoutLogAsync(string userId);

        Task<UserLogResult> GetUserLogListAsync(string userId, UserLogRequest request);
    }
}
