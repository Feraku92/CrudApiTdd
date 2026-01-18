using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using CrudApi.Application.Services;
using CrudApi.Domain.Entities;
using CrudApi.Domain.Exceptions;
using Moq;
using Xunit;

namespace CrudApi.Application.Tests.Services
{
    public class PokemonServiceTests
    {
        private readonly Mock<IPokemonRepsitory> _pokemonRepositoryMock;
        private readonly PokemonService _pokemonService;

        public PokemonServiceTests()
        {
            _pokemonRepositoryMock = new Mock<IPokemonRepsitory>();
            _pokemonService = new PokemonService(_pokemonRepositoryMock.Object);
        }

        [Fact]
        public void CreatePokemon_ShouldAddPokemon_WhenValidRequest()
        {
            // Arrange
            var request = new PokemonRequest(25, "Pikachu", "Electric", DateTime.UtcNow);

            _pokemonRepositoryMock.Setup(r => r.Add(It.IsAny<Pokemon>()));

            // Act
            var result = _pokemonService.CreatePokemon(request);

            // Assert
            _pokemonRepositoryMock.Verify(r => r.Add(It.IsAny<Pokemon>()), Times.Once);
            Assert.Equal(request.PokedexId, result.PokedexId);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Type, result.Type);
            Assert.Null(result.UpdatedAt);
        }

        [Fact]
        public void CreatePokemon_ShouldThrowException_WhenDuplicatePokedexId()
        {
            // Arrange
            var request = new PokemonRequest(25, "Pikachu", "Electric", DateTime.UtcNow);

            _pokemonRepositoryMock.Setup(r => r.Add(It.IsAny<Pokemon>()))
                .Throws(new DuplicatePokedexIdException(25));

            // Act & Assert
            Assert.Throws<DuplicatePokedexIdException>(() => _pokemonService.CreatePokemon(request));
        }

        [Fact]
        public void GetAllPokemons_ShouldReturnAllPokemons()
        {
            // Arrange
            var pokemons = new List<Pokemon>
            {
                new Pokemon(1, "Bulbasaur", "Grass", DateTime.UtcNow, null),
                new Pokemon(25, "Pikachu", "Electric", DateTime.UtcNow, null),
                new Pokemon(150, "Mewtwo", "Psychic", DateTime.UtcNow, null)
            };

            _pokemonRepositoryMock.Setup(r => r.GetAll()).Returns(pokemons);

            // Act
            var result = _pokemonService.GetAllPokemons();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Bulbasaur", result[0].Name);
            Assert.Equal("Pikachu", result[1].Name);
            Assert.Equal("Mewtwo", result[2].Name);
            _pokemonRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAllPokemons_ShouldReturnEmptyList_WhenNoPokemons()
        {
            // Arrange
            _pokemonRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Pokemon>());

            // Act
            var result = _pokemonService.GetAllPokemons();

            // Assert
            Assert.Empty(result);
            _pokemonRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetByNameOrNumber_ShouldReturnPokemon_WhenSearchByName()
        {
            // Arrange
            var pokemon = new Pokemon(25, "Pikachu", "Electric", DateTime.UtcNow, null);
            _pokemonRepositoryMock.Setup(r => r.GetByNameOrNumber("Pikachu", null))
                .Returns(pokemon);

            // Act
            var result = _pokemonService.GetByNameOrNumber("Pikachu", null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pikachu", result.Name);
            Assert.Equal(25, result.PokedexId);
            _pokemonRepositoryMock.Verify(r => r.GetByNameOrNumber("Pikachu", null), Times.Once);
        }

        [Fact]
        public void GetByNameOrNumber_ShouldReturnPokemon_WhenSearchByNumber()
        {
            // Arrange
            var pokemon = new Pokemon(25, "Pikachu", "Electric", DateTime.UtcNow, null);
            _pokemonRepositoryMock.Setup(r => r.GetByNameOrNumber(null, 25))
                .Returns(pokemon);

            // Act
            var result = _pokemonService.GetByNameOrNumber(null, 25);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pikachu", result.Name);
            Assert.Equal(25, result.PokedexId);
            _pokemonRepositoryMock.Verify(r => r.GetByNameOrNumber(null, 25), Times.Once);
        }

        [Fact]
        public void GetByNameOrNumber_ShouldReturnNull_WhenPokemonNotFound()
        {
            // Arrange
            _pokemonRepositoryMock.Setup(r => r.GetByNameOrNumber("Unknown", null))
                .Returns((Pokemon)null);

            // Act
            var result = _pokemonService.GetByNameOrNumber("Unknown", null);

            // Assert
            Assert.Null(result);
            _pokemonRepositoryMock.Verify(r => r.GetByNameOrNumber("Unknown", null), Times.Once);
        }

        [Fact]
        public void UpdatePokemon_ShouldUpdatePokemon_WhenPokemonExists()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var createdAt = DateTime.UtcNow.AddDays(-1);
            var request = new PokemonRequest(25, "Pikachu Updated", "Electric", createdAt);

            var updatedPokemon = new Pokemon(25, "Pikachu Updated", "Electric", createdAt, DateTime.UtcNow);
            _pokemonRepositoryMock.Setup(r => r.Update(id, It.IsAny<Pokemon>()))
                .Returns(updatedPokemon);

            // Act
            var result = _pokemonService.UpdatePokemon(id, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pikachu Updated", result.Name);
            Assert.NotNull(result.UpdatedAt);
            _pokemonRepositoryMock.Verify(r => r.Update(id, It.IsAny<Pokemon>()), Times.Once);
        }

        [Fact]
        public void UpdatePokemon_ShouldReturnNull_WhenPokemonNotFound()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var request = new PokemonRequest(25, "Pikachu", "Electric", DateTime.UtcNow);

            _pokemonRepositoryMock.Setup(r => r.Update(id, It.IsAny<Pokemon>()))
                .Returns((Pokemon)null);

            // Act
            var result = _pokemonService.UpdatePokemon(id, request);

            // Assert
            Assert.Null(result);
            _pokemonRepositoryMock.Verify(r => r.Update(id, It.IsAny<Pokemon>()), Times.Once);
        }

        [Fact]
        public void DeletePokemon_ShouldReturnTrue_WhenPokemonDeleted()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _pokemonRepositoryMock.Setup(r => r.Delete(id)).Returns(true);

            // Act
            var result = _pokemonService.DeletePokemon(id);

            // Assert
            Assert.True(result);
            _pokemonRepositoryMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public void DeletePokemon_ShouldReturnFalse_WhenPokemonNotFound()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _pokemonRepositoryMock.Setup(r => r.Delete(id)).Returns(false);

            // Act
            var result = _pokemonService.DeletePokemon(id);

            // Assert
            Assert.False(result);
            _pokemonRepositoryMock.Verify(r => r.Delete(id), Times.Once);
        }
    }
}
