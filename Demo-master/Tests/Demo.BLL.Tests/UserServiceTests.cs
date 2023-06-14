using AutoMapper;
using Demo.BLL.Service;
using Demo.DAL.IRepository;
using Demo.Infrastructure.Entity;
using Demo.Infrastructure.Mapper;
using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.ResultModel;
using Moq;

namespace Demo.BLL.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;

        #region Constructor
        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserMapper());
                });
                _mapper = mappingConfig.CreateMapper();
            }
        }
        #endregion

        #region AddUserLoginLogAsync Tests
        [Fact]
        public async Task AddUserLoginLogAsync_Invoke_UserRepository_AddUserLoginLogAsync()
        {
            // Arrange
            _mockUserRepository.Setup(s => s.AddUserLoginLogAsync(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(true);
            var service = GetTestInstance();

            // Act
            var result = await service.AddUserLoginLogAsync(true, "daa92639-db93-4a88-a758-7b3d2966f46d");

            //Assert
            _mockUserRepository.Verify(g => g.AddUserLoginLogAsync(It.IsAny<bool>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddUserLoginLogAsync_Returns_True_Successfully()
        {
            // Arrange
            _mockUserRepository.Setup(s => s.AddUserLoginLogAsync(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(true);
            var service = GetTestInstance();

            // Act
            var result = await service.AddUserLoginLogAsync(true, "daa92639-db93-4a88-a758-7b3d2966f46d");

            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result);
        }
        #endregion

        #region AddUserLogoutLogAsync Tests
        [Fact]
        public async Task AddUserLogoutLogAsync_Invoke_UserRepository_AddUserLogoutLogAsync()
        {
            // Arrange
            _mockUserRepository.Setup(s => s.AddUserLogoutLogAsync(It.IsAny<string>())).ReturnsAsync(true);
            var service = GetTestInstance();

            // Act
            var result = await service.AddUserLogoutLogAsync("daa92639-db93-4a88-a758-7b3d2966f46d");

            //Assert
            _mockUserRepository.Verify(g => g.AddUserLogoutLogAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddUserLogoutLogAsync_Returns_True_Successfully()
        {
            // Arrange
            _mockUserRepository.Setup(s => s.AddUserLogoutLogAsync(It.IsAny<string>())).ReturnsAsync(true);
            var service = GetTestInstance();

            // Act
            var result = await service.AddUserLogoutLogAsync("daa92639-db93-4a88-a758-7b3d2966f46d");

            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result);
        }
        #endregion

        #region GetUserListAsync Tests
        [Fact]
        public async Task GetUserListAsync_Invoke_UserRepository_GetUserListAsync()
        {
            // Arrange
            _mockUserRepository.Setup(s => s.GetUserListAsync(It.IsAny<GetUserListRequest>())).ReturnsAsync((GetTestUsers(), 2));
            var service = GetTestInstance();

            // Act
            var result = await service.GetUserListAsync(new GetUserListRequest() { });

            //Assert
            _mockUserRepository.Verify(g => g.GetUserListAsync(It.IsAny<GetUserListRequest>()), Times.Once);
        }

        [Fact]
        public async Task GetUserListAsync_Returns_UserResult_Successfully()
        {
            // Arrange
            var users = GetTestUsers();
            var pagedUsers = users.Take(2).ToList();
            var totalCount = users.Count;
            var itemsPerPage = 2;
            _mockUserRepository.Setup(s => s.GetUserListAsync(It.IsAny<GetUserListRequest>())).ReturnsAsync((pagedUsers, totalCount));
            var service = GetTestInstance();

            // Act
            var result = await service.GetUserListAsync(new GetUserListRequest() { PageNumber = 1, PageSize = 2 });

            //Assert
            Assert.IsType<UserResult>(result);
            Assert.Equal(itemsPerPage, result.MetaData?.ItemsPerPage);
            Assert.Equal(totalCount, result.MetaData?.TotalItems);
            Assert.Equal(Math.Ceiling(totalCount * 1M / itemsPerPage), result.MetaData?.TotalPages);
            Assert.Equal(pagedUsers.Count, result.Items?.Count);
        }
        #endregion

        #region GetUserLogListAsync Tests
        [Fact]
        public async Task GetUserLogListAsync_Invoke_UserRepository_GetUserLogListAsync()
        {
            // Arrange
            _mockUserRepository.Setup(s => s.GetUserLogListAsync(It.IsAny<string>(), It.IsAny<UserLogRequest>())).ReturnsAsync((GetTestUserLogs(), 10));
            var service = GetTestInstance();

            // Act
            var result = await service.GetUserLogListAsync("daa92639-db93-4a88-a758-7b3d2966f46d", new UserLogRequest() { });

            //Assert
            _mockUserRepository.Verify(g => g.GetUserLogListAsync(It.IsAny<string>(), It.IsAny<UserLogRequest>()), Times.Once);
        }

        [Fact]
        public async Task GetUserLogListAsync_Returns_UserLogResult_Successfully()
        {
            // Arrange
            var logs = GetTestUserLogs();
            var pagedLogs = logs.Take(2).ToList();
            var totalCount = logs.Count;
            var itemsPerPage = 2;
            _mockUserRepository.Setup(s => s.GetUserLogListAsync(It.IsAny<string>(), It.IsAny<UserLogRequest>())).ReturnsAsync((pagedLogs, totalCount));
            var service = GetTestInstance();

            // Act
            var result = await service.GetUserLogListAsync("daa92639-db93-4a88-a758-7b3d2966f46d", new UserLogRequest() { PageNumber = 1, PageSize = 2 });

            //Assert
            Assert.IsType<UserLogResult>(result);
            Assert.Equal(itemsPerPage, result.MetaData?.ItemsPerPage);
            Assert.Equal(totalCount, result.MetaData?.TotalItems);
            Assert.Equal(Math.Ceiling(totalCount * 1M / itemsPerPage), result.MetaData?.TotalPages);
            Assert.Equal(pagedLogs.Count, result.Items?.Count);
        }
        #endregion

        private List<User> GetTestUsers()
        {
            var users = new List<User>();
            users.Add(new User { FirstName = "A", LastName = "X", UserName = "a.x@gmail.com", Email = "a.x@gmail.com", PhoneNumber = "1234567890" });
            users.Add(new User { FirstName = "B", LastName = "Y", UserName = "b.y@gmail.com", Email = "b.y@gmail.com", PhoneNumber = "2345678901" });
            users.Add(new User { FirstName = "C", LastName = "Z", UserName = "c.z@gmail.com", Email = "c.z@gmail.com", PhoneNumber = "33456789012" });
            return users;
        }

        private List<UserLog> GetTestUserLogs()
        {
            var userLogs = new List<UserLog>();
            userLogs.Add(new UserLog() { Id = 1, IsSuccess = true, LoginTime = DateTime.Now.AddDays(-2), UserId = "daa92639-db93-4a88-a758-7b3d2966f46d" });
            userLogs.Add(new UserLog() { Id = 1, IsSuccess = true, LoginTime = DateTime.Now.AddDays(-1).AddMinutes(-30), LogoutTime = DateTime.Now.AddDays(-1), UserId = "daa92639-db93-4a88-a758-7b3d2966f46d" });
            userLogs.Add(new UserLog() { Id = 1, IsSuccess = false, LoginTime = DateTime.Now.AddDays(-4).AddMinutes(-30), UserId = "daa92639-db93-4a88-a758-7b3d2966f46d" });
            return userLogs;
        }

        private UserService GetTestInstance()
        {
            return new UserService(_mockUserRepository.Object, _mapper);
        }
    }
}