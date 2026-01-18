using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using CrudApi.Application.Services;
using CrudApi.Domain.Entities;
using Moq;
using Xunit;

namespace CrudApi.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;
        private readonly string _jwtSecret = "clave_super_segura_para_test";

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object, _jwtSecret);
        }

        [Fact]
        public void RegisterUser_ShouldAddUser_WhenEmailIsUnique()
        {
            var registerUserRequest = new RegisterUserRequest("testuser", "test@example.com", "password123");
            _userRepositoryMock.Setup(r => r.GetByUserName("test@example.com")).Returns((User)null);

            var result = _userService.RegisterUser(registerUserRequest);

            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
            Assert.Equal("testuser", result.UserName);
        }

        [Fact]
        public void RegisterUser_ShouldThrow_WhenUserNameExists()
        {
            var registerUserRequest = new RegisterUserRequest("testuser", "test@example.com", "password123");
            _userRepositoryMock.Setup(r => r.GetByUserName("testuser"))
                .Returns(new User("existing", "testuser", "pass"));

            Assert.Throws<InvalidOperationException>(() =>
                _userService.RegisterUser(registerUserRequest));
        }

        [Fact]
        public void AuthenticateUser_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var user = new User("testuser", "test@example.com", "password123");
            _userRepositoryMock.Setup(r => r.GetByUserName("testuser")).Returns(user);

            var result = _userService.AuthenticateUser("testuser", "password123");

            Assert.Equal("testuser", result);
        }

        [Fact]
        public void AuthenticateUser_ShouldThrow_WhenCredentialsInvalid()
        {
            var user = new User("testuser", "test@example.com", "fakepassword");
            _userRepositoryMock.Setup(r => r.GetByUserName("testuser")).Returns(user);

            Assert.Throws<UnauthorizedAccessException>(() =>
                _userService.AuthenticateUser("testuser", "wrongpassword"));
        }
    }
}
