using CrudApi.Application.Dtos;
using CrudApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace CrudApi.IntegrationTests
{
    public class PokemonIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public PokemonIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        public void Dispose()
        {
            _factory.CleanupDatabase();
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var username = $"pokemontest_{Guid.NewGuid()}";
            var registerRequest = new RegisterUserRequest(username, $"{username}@example.com", "password123");
            await _client.PostAsJsonAsync("/api/User/register", registerRequest);

            var loginRequest = new LoginRequest(username, "password123");
            var loginResponse = await _client.PostAsJsonAsync("/api/User/login", loginRequest);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return loginResult["token"];
        }

        [Fact]
        public async Task GetAll_ShouldReturnUnauthorized_WithoutToken()
        {
            // Act
            var response = await _client.GetAsync("/api/Pokemon/getall");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithValidToken()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/Pokemon/getall");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WithoutToken()
        {
            // Arrange
            var request = new PokemonRequest(999, "TestPokemon", "Test", DateTime.UtcNow);

            // Act
            var response = await _client.PostAsJsonAsync("/api/Pokemon/create", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WithValidToken()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new PokemonRequest(
                new Random().Next(1000, 9999),
                $"TestPokemon_{Guid.NewGuid()}",
                "Test",
                DateTime.UtcNow
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/Pokemon/create", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var pokemon = await response.Content.ReadFromJsonAsync<Pokemon>();
            Assert.Equal(request.Name, pokemon.Name);
            Assert.Equal(request.PokedexId, pokemon.PokedexId);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenDuplicatePokedexId()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var pokedexId = new Random().Next(10000, 99999);
            var request1 = new PokemonRequest(
                pokedexId,
                $"Pokemon1_{Guid.NewGuid()}",
                "Test",
                DateTime.UtcNow
            );

            var request2 = new PokemonRequest(
                pokedexId,
                $"Pokemon2_{Guid.NewGuid()}",
                "Test",
                DateTime.UtcNow
            );

            // Act
            await _client.PostAsJsonAsync("/api/Pokemon/create", request1);
            var response = await _client.PostAsJsonAsync("/api/Pokemon/create", request2);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetByNameOrNumber_ShouldReturnOk_WhenPokemonExists()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createRequest = new PokemonRequest(
                new Random().Next(100000, 999999),
                $"SearchTest_{Guid.NewGuid()}",
                "Test",
                DateTime.UtcNow
            );
            await _client.PostAsJsonAsync("/api/Pokemon/create", createRequest);

            // Act
            var response = await _client.GetAsync($"/api/Pokemon/search?name={createRequest.Name}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var pokemon = await response.Content.ReadFromJsonAsync<Pokemon>();
            Assert.Equal(createRequest.Name, pokemon.Name);
        }

        [Fact]
        public async Task GetByNameOrNumber_ShouldReturnNotFound_WhenPokemonDoesNotExist()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync($"/api/Pokemon/search?name=NonExistent{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenPokemonExists()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createRequest = new PokemonRequest(
                new Random().Next(1000000, 9999999),
                $"UpdateTest_{Guid.NewGuid()}",
                "Test",
                DateTime.UtcNow
            );
            var createResponse = await _client.PostAsJsonAsync("/api/Pokemon/create", createRequest);
            var createdPokemon = await createResponse.Content.ReadFromJsonAsync<Pokemon>();

            var updateRequest = new PokemonRequest(
                createRequest.PokedexId,
                "UpdatedName",
                "UpdatedType",
                createdPokemon.CreatedAt
            );

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Pokemon/{createdPokemon.Id}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var updatedPokemon = await response.Content.ReadFromJsonAsync<Pokemon>();
            Assert.Equal("UpdatedName", updatedPokemon.Name);
            Assert.Equal("UpdatedType", updatedPokemon.Type);
            Assert.NotNull(updatedPokemon.UpdatedAt);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenPokemonDoesNotExist()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var updateRequest = new PokemonRequest(1, "UpdatedName", "UpdatedType", DateTime.UtcNow);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Pokemon/{Guid.NewGuid()}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenPokemonExists()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createRequest = new PokemonRequest(
                new Random().Next(10000000, 99999999),
                $"DeleteTest_{Guid.NewGuid()}",
                "Test",
                DateTime.UtcNow
            );
            var createResponse = await _client.PostAsJsonAsync("/api/Pokemon/create", createRequest);
            var createdPokemon = await createResponse.Content.ReadFromJsonAsync<Pokemon>();

            // Act
            var response = await _client.DeleteAsync($"/api/Pokemon/{createdPokemon.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenPokemonDoesNotExist()
        {
            // Arrange
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.DeleteAsync($"/api/Pokemon/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
