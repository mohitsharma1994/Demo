using Demo.Infrastructure.Entity;
using Demo.Infrastructure.RequestModel;

namespace Demo.DAL.IRepository
{
    public interface IUserRepository
    {
        Task<(List<User>, int)> GetUserListAsync(GetUserListRequest request);

        Task<bool> AddUserLoginLogAsync(bool isSuccess, string userId);

        Task<bool> AddUserLogoutLogAsync(string userId);

        Task<(List<UserLog>, int)> GetUserLogListAsync(string userId, UserLogRequest request);
    }
}
