using CrudApi.Application.Dtos;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CrudApi.IntegrationTests
{
    public class UserIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UserIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        public void Dispose()
        {
            _factory.CleanupDatabase();
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenValidRequest()
        {
            // Arrange
            var request = new RegisterUserRequest(
                $"testuser_{Guid.NewGuid()}",
                $"test_{Guid.NewGuid()}@example.com",
                "password123"
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/User/register", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(request.UserName, content);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenDuplicateUser()
        {
            // Arrange
            var username = $"duplicate_{Guid.NewGuid()}";
            var request1 = new RegisterUserRequest(username, $"{username}@example.com", "password123");
            var request2 = new RegisterUserRequest(username, $"{username}@example.com", "password123");

            // Act
            await _client.PostAsJsonAsync("/api/User/register", request1);
            var response = await _client.PostAsJsonAsync("/api/User/register", request2);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenValidCredentials()
        {
            // Arrange
            var username = $"logintest_{Guid.NewGuid()}";
            var password = "password123";
            var registerRequest = new RegisterUserRequest(username, $"{username}@example.com", password);
            await _client.PostAsJsonAsync("/api/User/register", registerRequest);

            var loginRequest = new LoginRequest(username, password);

            // Act
            var response = await _client.PostAsJsonAsync("/api/User/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("token", content);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            // Arrange
            var loginRequest = new LoginRequest("nonexistent", "wrongpassword");

            // Act
            var response = await _client.PostAsJsonAsync("/api/User/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenWrongPassword()
        {
            // Arrange
            var username = $"passwordtest_{Guid.NewGuid()}";
            var registerRequest = new RegisterUserRequest(username, $"{username}@example.com", "password123");
            await _client.PostAsJsonAsync("/api/User/register", registerRequest);

            var loginRequest = new LoginRequest(username, "wrongpassword");

            // Act
            var response = await _client.PostAsJsonAsync("/api/User/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
