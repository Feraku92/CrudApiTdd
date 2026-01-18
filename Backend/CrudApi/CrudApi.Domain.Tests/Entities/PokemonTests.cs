using CrudApi.Domain.Entities;
using Xunit;

namespace CrudApi.Domain.Tests.Entities
{
    public class PokemonTests
    {
        [Fact]
        public void CreatePokemon_WithValidData_ShouldSetProperties()
        {
            // Arrange
            var pokedexId = 25;
            var name = "Pikachu";
            var type = "Electric";
            var createdAt = DateTime.UtcNow;
            DateTime? updatedAt = null;

            // Act
            var pokemon = new Pokemon(pokedexId, name, type, createdAt, updatedAt);

            // Assert
            Assert.Equal(pokedexId, pokemon.PokedexId);
            Assert.Equal(name, pokemon.Name);
            Assert.Equal(type, pokemon.Type);
            Assert.Equal(createdAt, pokemon.CreatedAt);
            Assert.Null(pokemon.UpdatedAt);
        }

        [Fact]
        public void CreatePokemon_WithZeroPokedexId_ShouldThrowException()
        {
            // Arrange
            var pokedexId = 0;
            var name = "Pikachu";
            var type = "Electric";
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("PokedexId must be greater than 0", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithNegativePokedexId_ShouldThrowException()
        {
            // Arrange
            var pokedexId = -1;
            var name = "Pikachu";
            var type = "Electric";
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("PokedexId must be greater than 0", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithEmptyName_ShouldThrowException()
        {
            // Arrange
            var pokedexId = 25;
            var name = "";
            var type = "Electric";
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("Name cannot be empty", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithWhitespaceName_ShouldThrowException()
        {
            // Arrange
            var pokedexId = 25;
            var name = "   ";
            var type = "Electric";
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("Name cannot be empty", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithNullName_ShouldThrowException()
        {
            // Arrange
            var pokedexId = 25;
            string name = null;
            var type = "Electric";
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("Name cannot be empty", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithEmptyType_ShouldThrowException()
        {
            // Arrange
            var pokedexId = 25;
            var name = "Pikachu";
            var type = "";
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("Type cannot be empty", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithWhitespaceType_ShouldThrowException()
        {
            // Arrange
            var pokedexId = 25;
            var name = "Pikachu";
            var type = "   ";
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("Type cannot be empty", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithNullType_ShouldThrowException()
        {
            // Arrange
            var pokedexId = 25;
            var name = "Pikachu";
            string type = null;
            var createdAt = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Pokemon(pokedexId, name, type, createdAt, null));
            Assert.Contains("Type cannot be empty", exception.Message);
        }

        [Fact]
        public void CreatePokemon_WithUpdatedAt_ShouldSetUpdatedAtProperty()
        {
            // Arrange
            var pokedexId = 25;
            var name = "Pikachu";
            var type = "Electric";
            var createdAt = DateTime.UtcNow.AddDays(-1);
            var updatedAt = DateTime.UtcNow;

            // Act
            var pokemon = new Pokemon(pokedexId, name, type, createdAt, updatedAt);

            // Assert
            Assert.NotNull(pokemon.UpdatedAt);
            Assert.Equal(updatedAt, pokemon.UpdatedAt);
        }

        [Fact]
        public void CreatePokemon_ShouldGenerateUniqueId()
        {
            // Arrange & Act
            var pokemon1 = new Pokemon(1, "Bulbasaur", "Grass", DateTime.UtcNow, null);
            var pokemon2 = new Pokemon(2, "Ivysaur", "Grass", DateTime.UtcNow, null);

            // Assert
            Assert.NotEqual(Guid.Empty, pokemon1.Id);
            Assert.NotEqual(Guid.Empty, pokemon2.Id);
            Assert.NotEqual(pokemon1.Id, pokemon2.Id);
        }
    }
}
