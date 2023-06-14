using Demo.DAL.IRepository;
using Demo.Infrastructure.Entity;
using Demo.Infrastructure.RequestModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Demo.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserLoginLogAsync(bool isSuccess, string userId)
        {
            var userLog = new UserLog
            {
                IsSuccess = isSuccess,
                UserId = userId,
                LoginTime = DateTime.Now,
                LogoutTime = null
            };
            _context.UserLog.Add(userLog);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddUserLogoutLogAsync(string userId)
        {
            var userLastLog = await _context.UserLog.Where(x => x.UserId == userId).OrderByDescending(x => x.LoginTime).FirstOrDefaultAsync();
            if (userLastLog == null)
            {
                return false;
            }
            userLastLog.LogoutTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(List<User>, int)> GetUserListAsync(GetUserListRequest request)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x => x.UserName.ToUpper().Contains(request.Search.ToUpper())
                                         || (!string.IsNullOrWhiteSpace(x.FirstName) && x.FirstName.ToUpper().Contains(request.Search.ToUpper()))
                                         || (!string.IsNullOrWhiteSpace(x.LastName) && x.LastName.ToUpper().Contains(request.Search.ToUpper()))
                                         || (!string.IsNullOrWhiteSpace(x.PhoneNumber) && x.PhoneNumber.ToUpper().Contains(request.Search.ToUpper()))
                );
            }

            var totalCount = query.Count();

            if (!string.IsNullOrWhiteSpace(request.Sort))
            {
                var sortDirection = request.SortDirection ?? "ASC";
                return (await query.OrderBy(request.Sort + " " + sortDirection)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(), totalCount);
            }
            else
            {
                return (await query.OrderBy(x => x.UserName)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize).ToListAsync(), totalCount);
            }
        }

        public async Task<(List<UserLog>, int)> GetUserLogListAsync(string userId, UserLogRequest request)
        {
            var query = _context.UserLog.Where(x => x.UserId == userId).AsQueryable();
            var totalCount = query.Count();
            if (!string.IsNullOrWhiteSpace(request.Sort))
            {
                var sortDirection = request.SortDirection ?? "ASC";
                return (await query.OrderBy(request.Sort + " " + sortDirection)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(), totalCount);
            }
            else
            {
                return (await query.OrderByDescending(x => x.LoginTime)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize).ToListAsync(), totalCount);
            }
        }
    }
}
