using Demo.DAL.Repository;
using Demo.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Tests
{
    public class UserRepositoryTests
    {
        private readonly DataContext dbContext;

        #region Constrcutor
        public UserRepositoryTests()
        {
            var option = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;
            dbContext = new DataContext(option);
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            if (!dbContext.Users.Any())
            {
                dbContext.Users.Add(new User
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Email = "Test@gmail.com",
                    UserName = "Test@gmail.com",
                    PhoneNumber = "12364479",
                });

                dbContext.Users.Add(new User
                {
                    FirstName = "ABC",
                    LastName = "ABC",
                    Email = "ABC@gmail.com",
                    UserName = "ABC@gmail.com",
                    PhoneNumber = "2212364479",
                });
                dbContext.Users.Add(new User
                {
                    FirstName = "XYZ",
                    LastName = "XYZ",
                    Email = "XYZ@gmail.com",
                    UserName = "XYZ@gmail.com",
                    PhoneNumber = "2212364479",
                });
                dbContext.SaveChanges();
            }

            if(!dbContext.UserLog.Any())
            {
                var userId = dbContext.Users.First().Id;
                dbContext.UserLog.Add(new UserLog()
                {
                    IsSuccess = true,
                    LoginTime = DateTime.Now.AddDays(-2).AddMinutes(-30),
                    UserId = userId
                });
                dbContext.UserLog.Add(new UserLog()
                {
                    IsSuccess = true,
                    LoginTime = DateTime.Now.AddDays(-3).AddMinutes(-30),
                    LogoutTime = DateTime.Now.AddDays(-3).AddMinutes(-15),
                    UserId = userId
                });
                dbContext.UserLog.Add(new UserLog()
                {
                    IsSuccess = true,
                    LoginTime = DateTime.Now.AddMinutes(-30),
                    LogoutTime = DateTime.Now.AddMinutes(-15),
                    UserId = userId
                });
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region AddUserLoginLogAsync Tests
        [Fact]
        public async Task AddUserLoginLogAsync_Adds_LoginLog_Successfully()
        {
            // Arrange
            var userId = dbContext.Users.First().Id;
            var repository = GetTestInstance();
            var previousLogCount = dbContext.UserLog.Where(x => x.UserId == userId).Count();

            // Act
            var result = await repository.AddUserLoginLogAsync(true, userId);

            //Assert
            var count = dbContext.UserLog.Where(x => x.UserId == userId).Count();
            Assert.True(count > previousLogCount);
        }
        #endregion

        #region AddUserLogoutLogAsync Tests
        [Fact]
        public async Task AddUserLogoutLogAsync_InvalidUserid_Returns_False()
        {
            // Arrange
            var userId = "testid";
            var repository = GetTestInstance();

            // Act
            var result = await repository.AddUserLogoutLogAsync(userId);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddUserLogoutLogAsync_Adds_Logout_Successfully()
        {
            // Arrange
            var userId = dbContext.Users.First().Id;
            var repository = GetTestInstance();
            var v = await repository.AddUserLoginLogAsync(true, userId);

            // Act
            var result = await repository.AddUserLogoutLogAsync(userId);

            //Assert
            var log = dbContext.UserLog.Where(x => x.UserId == userId).OrderByDescending(x => x.LoginTime).First();
            Assert.NotNull(log.LogoutTime);
        }
        #endregion

        #region GetUserListAsync Tests
        [Fact]
        public async Task GetUserListAsync_Returns_UsersList_Successfully()
        {
            // Arrange
            var totalCount = dbContext.Users.Count();
            var pageCount = 2;
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserListAsync(new Infrastructure.RequestModel.GetUserListRequest()
            {
                PageNumber = 1,
                PageSize = pageCount,
            });

            //Assert
            Assert.Equal(totalCount, result.Item2);
            Assert.NotEmpty(result.Item1);
            if (totalCount > pageCount)
            {
                Assert.Equal(pageCount, result.Item1.Count);
            }
            else
            {
                Assert.True(result.Item1.Count > 0 && result.Item1.Count <= pageCount);
            }
        }

        [Fact]
        public async Task GetUserListAsync_Filters_UsersList_Successfully()
        {
            // Arrange
            var search = "XYZ";
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserListAsync(new Infrastructure.RequestModel.GetUserListRequest()
            {
                PageNumber = 1,
                PageSize = 10,
                Search = search
            });

            //Assert
            Assert.NotEmpty(result.Item1);
            Assert.True(result.Item1[0].FirstName.Contains(search)
                || result.Item1[0].LastName.Contains(search)
                || result.Item1[0].PhoneNumber.Contains(search));
        }

        [Fact]
        public async Task GetUserListAsync_Sort_UsersList_Successfully()
        {
            // Arrange
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserListAsync(new Infrastructure.RequestModel.GetUserListRequest()
            {
                PageNumber = 1,
                PageSize = 10,
                Sort = "FirstName",
                SortDirection = "DESC"
            });

            //Assert
            Assert.NotEmpty(result.Item1);
            Assert.Equal("XYZ", result.Item1[0].FirstName);
        }

        [Fact]
        public async Task GetUserListAsync_SortWithoutSortDirection_UsersList_Successfully()
        {
            // Arrange
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserListAsync(new Infrastructure.RequestModel.GetUserListRequest()
            {
                PageNumber = 1,
                PageSize = 10,
                Sort = "FirstName",
            });

            //Assert
            Assert.NotEmpty(result.Item1);
            Assert.Equal("ABC", result.Item1[0].FirstName);
        }

        [Fact]
        public async Task GetUserListAsync_Sort_UsersList_ByDefault_On_Username_Successfully()
        {
            // Arrange
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserListAsync(new Infrastructure.RequestModel.GetUserListRequest()
            {
                PageNumber = 1,
                PageSize = 10,
            });

            //Assert
            Assert.NotEmpty(result.Item1);
            Assert.Equal("ABC", result.Item1[0].FirstName);
        }
        #endregion


        #region GetUserLogListAsync Tests
        [Fact]
        public async Task GetUserLogListAsync_Returns_UserLog_Successfully()
        {
            // Arrange
            var userId = dbContext.Users.First().Id;
            var totalCount = dbContext.UserLog.Where(x => x.UserId == userId).Count();
            var pageCount = 2;
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserLogListAsync(userId, new Infrastructure.RequestModel.UserLogRequest()
            {
                PageNumber = 1,
                PageSize = pageCount,
            });

            //Assert
            Assert.Equal(totalCount, result.Item2);
            Assert.NotEmpty(result.Item1);
            if (totalCount > pageCount)
            {
                Assert.Equal(pageCount, result.Item1.Count);
            }
            else
            {
                Assert.True(result.Item1.Count > 0 && result.Item1.Count <= pageCount);
            }
            Assert.Equal(userId, result.Item1[0].UserId);
        }

        [Fact]
        public async Task GetUserLogListAsync_Sort_UserLog_Successfully()
        {
            // Arrange
            var userId = dbContext.Users.First().Id;
            var pageCount = 2;
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserLogListAsync(userId, new Infrastructure.RequestModel.UserLogRequest()
            {
                PageNumber = 1,
                PageSize = pageCount,
                Sort = "LogoutTime"

            });

            //Assert
            Assert.NotEmpty(result.Item1);
            Assert.Null(result.Item1[0].LogoutTime);
        }

        [Fact]
        public async Task GetUserLogListAsync_Sort_UsersList_ByDefault_On_Username_Successfully()
        {
            // Arrange
            var userId = dbContext.Users.First().Id;
            var latestLoginTime = dbContext.UserLog.Where(x => x.UserId == userId).OrderByDescending(x => x.LoginTime).First().LoginTime;
            var repository = GetTestInstance();

            // Act
            var result = await repository.GetUserLogListAsync(userId, new Infrastructure.RequestModel.UserLogRequest()
            {
                PageNumber = 1,
                PageSize = 10,
            });

            //Assert
            Assert.NotEmpty(result.Item1);
            Assert.Equal(latestLoginTime, result.Item1[0].LoginTime);
        }
        #endregion

        private UserRepository GetTestInstance()
        {
            return new UserRepository(dbContext);
        }
    }
}