using CrudApi.Api.Controllers;
using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;
using CrudApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrudApi.Api.Tests.Controllers
{
    public class PokemonControllerTests
    {
        private readonly Mock<IPokemonService> _pokemonServiceMock;
        private readonly PokemonController _controller;

        public PokemonControllerTests()
        {
            _pokemonServiceMock = new Mock<IPokemonService>();
            _controller = new PokemonController(_pokemonServiceMock.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOk_WithAllPokemons()
        {
            // Arrange
            var pokemons = new List<Pokemon>
            {
                new Pokemon(1, "Bulbasaur", "Grass", DateTime.UtcNow, null),
                new Pokemon(25, "Pikachu", "Electric", DateTime.UtcNow, null)
            };
            _pokemonServiceMock.Setup(s => s.GetAllPokemons()).Returns(pokemons);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPokemons = Assert.IsAssignableFrom<List<Pokemon>>(okResult.Value);
            Assert.Equal(2, returnedPokemons.Count);
            _pokemonServiceMock.Verify(s => s.GetAllPokemons(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnOk_WithEmptyList_WhenNoPokemons()
        {
            // Arrange
            _pokemonServiceMock.Setup(s => s.GetAllPokemons()).Returns(new List<Pokemon>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPokemons = Assert.IsAssignableFrom<List<Pokemon>>(okResult.Value);
            Assert.Empty(returnedPokemons);
        }

        [Fact]
        public void GetByNameOrNumber_ShouldReturnOk_WhenPokemonFound()
        {
            // Arrange
            var pokemon = new Pokemon(25, "Pikachu", "Electric", DateTime.UtcNow, null);
            _pokemonServiceMock.Setup(s => s.GetByNameOrNumber("Pikachu", null)).Returns(pokemon);

            // Act
            var result = _controller.GetByNameOrNumber("Pikachu", null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPokemon = Assert.IsType<Pokemon>(okResult.Value);
            Assert.Equal("Pikachu", returnedPokemon.Name);
            _pokemonServiceMock.Verify(s => s.GetByNameOrNumber("Pikachu", null), Times.Once);
        }

        [Fact]
        public void GetByNameOrNumber_ShouldReturnNotFound_WhenPokemonNotFound()
        {
            // Arrange
            _pokemonServiceMock.Setup(s => s.GetByNameOrNumber("Unknown", null)).Returns((Pokemon)null);

            // Act
            var result = _controller.GetByNameOrNumber("Unknown", null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public void GetByNameOrNumber_ShouldSearchByNumber_WhenNumberProvided()
        {
            // Arrange
            var pokemon = new Pokemon(25, "Pikachu", "Electric", DateTime.UtcNow, null);
            _pokemonServiceMock.Setup(s => s.GetByNameOrNumber(null, 25)).Returns(pokemon);

            // Act
            var result = _controller.GetByNameOrNumber(null, 25);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPokemon = Assert.IsType<Pokemon>(okResult.Value);
            Assert.Equal(25, returnedPokemon.PokedexId);
            _pokemonServiceMock.Verify(s => s.GetByNameOrNumber(null, 25), Times.Once);
        }

        [Fact]
        public void Create_ShouldReturnOk_WhenPokemonCreated()
        {
            // Arrange
            var request = new PokemonRequest(25, "Pikachu", "Electric", DateTime.UtcNow);
            var pokemon = new Pokemon(25, "Pikachu", "Electric", DateTime.UtcNow, null);
            _pokemonServiceMock.Setup(s => s.CreatePokemon(request)).Returns(pokemon);

            // Act
            var result = _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPokemon = Assert.IsType<Pokemon>(okResult.Value);
            Assert.Equal("Pikachu", returnedPokemon.Name);
            _pokemonServiceMock.Verify(s => s.CreatePokemon(request), Times.Once);
        }

        [Fact]
        public void Create_ShouldReturnBadRequest_WhenDuplicatePokedexId()
        {
            // Arrange
            var request = new PokemonRequest(25, "Pikachu", "Electric", DateTime.UtcNow);
            _pokemonServiceMock.Setup(s => s.CreatePokemon(request))
                .Throws(new DuplicatePokedexIdException(25));

            // Act
            var result = _controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Update_ShouldReturnOk_WhenPokemonUpdated()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var request = new PokemonRequest(25, "Pikachu Updated", "Electric", DateTime.UtcNow.AddDays(-1));
            var updatedPokemon = new Pokemon(25, "Pikachu Updated", "Electric", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            _pokemonServiceMock.Setup(s => s.UpdatePokemon(id, request)).Returns(updatedPokemon);

            // Act
            var result = _controller.Update(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPokemon = Assert.IsType<Pokemon>(okResult.Value);
            Assert.Equal("Pikachu Updated", returnedPokemon.Name);
            Assert.NotNull(returnedPokemon.UpdatedAt);
            _pokemonServiceMock.Verify(s => s.UpdatePokemon(id, request), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnNotFound_WhenPokemonNotFound()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var request = new PokemonRequest(25, "Pikachu", "Electric", DateTime.UtcNow);
            _pokemonServiceMock.Setup(s => s.UpdatePokemon(id, request)).Returns((Pokemon)null);

            // Act
            var result = _controller.Update(id, request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent_WhenPokemonDeleted()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _pokemonServiceMock.Setup(s => s.DeletePokemon(id)).Returns(true);

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _pokemonServiceMock.Verify(s => s.DeletePokemon(id), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenPokemonNotFound()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _pokemonServiceMock.Setup(s => s.DeletePokemon(id)).Returns(false);

            // Act
            var result = _controller.Delete(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
