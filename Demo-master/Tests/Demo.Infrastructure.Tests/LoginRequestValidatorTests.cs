using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.Validator;

namespace Demo.Infrastructure.Tests
{
    public class LoginRequestValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void LoginRequest_ShouldHave_ValidationError_Given_UserName_ThatIs_NullEmptyOrWhitespace(string userName)
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new LoginRequest
            {
                UserName = userName,
                Password = "Test1113"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "UserName");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void LoginRequest_ShouldHave_ValidationError_Given_Password_ThatIs_NullEmptyOrWhitespace(string password)
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new LoginRequest
            {
                UserName = "Testt1232",
                Password = password
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "Password");
        }

        private LoginRequestValidator GetTestInstance()
        {
            return new LoginRequestValidator();
        }
    }
}