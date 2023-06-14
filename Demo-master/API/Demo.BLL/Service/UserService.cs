using AutoMapper;
using Demo.BLL.IService;
using Demo.DAL.IRepository;
using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.ResultModel;

namespace Demo.BLL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddUserLoginLogAsync(bool isSuccess, string userId)
        {
            return await _userRepository.AddUserLoginLogAsync(isSuccess, userId);
        }

        public async Task<bool> AddUserLogoutLogAsync(string userId)
        {
            return await _userRepository.AddUserLogoutLogAsync(userId);
        }

        public async Task<UserResult> GetUserListAsync(GetUserListRequest request)
        {
            var result = await _userRepository.GetUserListAsync(request);
            var items= _mapper.Map<List<UserResultItem>>(result.Item1);
            return new UserResult()
            {
                Items = items,
                MetaData = new PaginatedMetaData()
                {
                    TotalItems = result.Item2,
                    ItemsPerPage = request.PageSize
                }
            };
        }

        public async Task<UserLogResult> GetUserLogListAsync(string userId, UserLogRequest request)
        {
            var result = await _userRepository.GetUserLogListAsync(userId, request);
            var items = _mapper.Map<List<UserLogResultItem>>(result.Item1);
            return new UserLogResult()
            {
                Items = items,
                MetaData = new PaginatedMetaData()
                {
                    TotalItems = result.Item2,
                    ItemsPerPage = request.PageSize
                }
            };
        }
    }
}
