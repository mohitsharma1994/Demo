using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.Validator;

namespace Demo.Infrastructure.Tests
{
    public class CreateUserValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CreateUserModel_ShouldHave_ValidationError_Given_FirstName_ThatIs_NullEmptyOrWhitespace(string name)
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = name,
                LastName = "LastName",
                Email = "test@test.com",
                Password = "Test@123"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "FirstName");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CreateUserModel_ShouldHave_ValidationError_Given_LastName_ThatIs_NullEmptyOrWhitespace(string name)
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = name,
                Email = "test@test.com",
                Password = "Test@123"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "LastName");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CreateUserModel_ShouldHave_ValidationError_Given_Email_ThatIs_NullEmptyOrWhitespace(string email)
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = email,
                Password = "Test@123"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "Email");
        }

        [Theory]
        [InlineData("jghghjhj")]
        [InlineData("abc@")]
        public void CreateUserModel_ShouldHave_ValidationError_Given_Email_ThatIs_Invalid(string email)
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = email,
                Password = "Test@123"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "Email");
            Assert.Equal($"A valid email is required", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateUserModel_ShouldNotHave_ValidationError_Given_Email_ThatIs_Valid()
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Password = "Test@123"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CreateUserModel_ShouldHave_ValidationError_Given_Password_ThatIs_NullEmptyOrWhitespace(string password)
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Password = password
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "Password");
        }

        [Fact]
        public void CreateUserModel_ShouldHave_ValidationError_Given_Password_ThatIs_LessThan6Chars()
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Password = "123"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "Password");
            Assert.Equal($"Your password length must be at least 6 characters.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateUserModel_ShouldHave_ValidationError_Given_Password_ThatIs_NotHavingUpperCase()
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Password = "abcedhjhfd"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "Password");
            Assert.Equal($"Your password must contain at least one uppercase letter.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateUserModel_ShouldHave_ValidationError_Given_Password_ThatIs_NotHavingOneNumber()
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Password = "Abcedhjhfd"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, x => x.PropertyName == "Password");
            Assert.Equal($"Your password must contain at least one number.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateUserModel_ShouldNotHave_ValidationError_Given_Password_ThatIs_ValidPassword()
        {
            //Arrange
            var sut = GetTestInstance();
            var request = new CreateUserModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                Password = "Abcedhjhfd12"
            };

            //Act & Assert
            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }

        private CreateUserValidator GetTestInstance()
        {
            return new CreateUserValidator();
        }
    }
}