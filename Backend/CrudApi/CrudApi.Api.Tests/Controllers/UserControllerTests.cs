using CrudApi.Api.Controllers;
using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrudApi.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public void Register_ShouldReturnOk_WhenRegistrationSuccessful()
        {
            // Arrange
            var request = new RegisterUserRequest("testuser", "test@example.com", "password123");
            var user = new User("testuser", "test@example.com", "password123");

            _userServiceMock.Setup(s => s.RegisterUser(request)).Returns(user);

            // Act
            var result = _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _userServiceMock.Verify(s => s.RegisterUser(request), Times.Once);
        }

        [Fact]
        public void Register_ShouldReturnBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var request = new RegisterUserRequest("testuser", "test@example.com", "password123");

            _userServiceMock.Setup(s => s.RegisterUser(request))
                .Throws(new InvalidOperationException("User already exists"));

            // Act
            var result = _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            _userServiceMock.Verify(s => s.RegisterUser(request), Times.Once);
        }

        [Fact]
        public void Register_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var request = new RegisterUserRequest("testuser", "test@example.com", "password123");

            _userServiceMock.Setup(s => s.RegisterUser(request))
                .Throws(new Exception("Registration failed"));

            // Act
            var result = _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginRequest("testuser", "password123");
            var token = "valid-jwt-token";

            _userServiceMock.Setup(s => s.AuthenticateUser(request.UserName, request.Password))
                .Returns(token);

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _userServiceMock.Verify(s => s.AuthenticateUser(request.UserName, request.Password), Times.Once);
        }

        [Fact]
        public void Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new LoginRequest("testuser", "wrongpassword");

            _userServiceMock.Setup(s => s.AuthenticateUser(request.UserName, request.Password))
                .Throws(new UnauthorizedAccessException("Invalid credentials"));

            // Act
            var result = _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.NotNull(unauthorizedResult.Value);
            _userServiceMock.Verify(s => s.AuthenticateUser(request.UserName, request.Password), Times.Once);
        }

        [Fact]
        public void Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var request = new LoginRequest("nonexistent", "password123");

            _userServiceMock.Setup(s => s.AuthenticateUser(request.UserName, request.Password))
                .Throws(new UnauthorizedAccessException("User not found"));

            // Act
            var result = _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.NotNull(unauthorizedResult.Value);
        }

        [Fact]
        public void Login_ShouldReturnUnauthorized_WhenExceptionThrown()
        {
            // Arrange
            var request = new LoginRequest("testuser", "password123");

            _userServiceMock.Setup(s => s.AuthenticateUser(request.UserName, request.Password))
                .Throws(new Exception("Authentication failed"));

            // Act
            var result = _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.NotNull(unauthorizedResult.Value);
        }
    }
}
