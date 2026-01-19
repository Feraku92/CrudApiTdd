using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CrudApi.Infrastructure.DataSeeder
{
    public class DatabaseSeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IPokemonRepsitory _pokemonRepository;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(
            IUserRepository userRepository,
            IPokemonRepsitory pokemonRepository,
            ILogger<DatabaseSeeder> logger)
        {
            _userRepository = userRepository;
            _pokemonRepository = pokemonRepository;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedUsersAsync();
                await SeedPokemonAsync();
                _logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
            }
        }

        private async Task SeedUsersAsync()
        {
            // Check if users already exist
            var existingUser = _userRepository.GetByUserName("trainer_ash");
            if (existingUser != null)
            {
                _logger.LogInformation("Demo user already exists. Skipping user seeding.");
                return;
            }

            _logger.LogInformation("Seeding demo users...");

            // Demo User 1: Ash
            var ashUser = new User(
                username: "trainer_ash",
                email: "ash@pokemon.com",
                password: BCrypt.Net.BCrypt.HashPassword("Pikachu123!")
            );

            // Demo User 2: Misty
            var mistyUser = new User(
                username: "trainer_misty",
                email: "misty@pokemon.com",
                password: BCrypt.Net.BCrypt.HashPassword("Starmie456!")
            );

            // Demo User 3: Brock
            var brockUser = new User(
                username: "trainer_brock",
                email: "brock@pokemon.com",
                password: BCrypt.Net.BCrypt.HashPassword("Onix789!")
            );

            _userRepository.Add(ashUser);
            _userRepository.Add(mistyUser);
            _userRepository.Add(brockUser);

            _logger.LogInformation("Demo users created successfully");
        }

        private async Task SeedPokemonAsync()
        {
            // Check if Pokémon already exist
            var existingPokemons = _pokemonRepository.GetAll();
            if (existingPokemons.Any())
            {
                _logger.LogInformation("Pokémon data already exists. Skipping Pokémon seeding.");
                return;
            }

            _logger.LogInformation("Seeding demo Pokémon...");

            var demoPokemons = new List<Pokemon>
            {
                new Pokemon(
                    pokedexid: 1,
                    name: "Bulbasaur",
                    type: "Grass/Poison",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 4,
                    name: "Charmander",
                    type: "Fire",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 7,
                    name: "Squirtle",
                    type: "Water",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 25,
                    name: "Pikachu",
                    type: "Electric",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 6,
                    name: "Charizard",
                    type: "Fire/Flying",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 9,
                    name: "Blastoise",
                    type: "Water",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 150,
                    name: "Mewtwo",
                    type: "Psychic",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 151,
                    name: "Mew",
                    type: "Psychic",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 94,
                    name: "Gengar",
                    type: "Ghost/Poison",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                ),
                new Pokemon(
                    pokedexid: 143,
                    name: "Snorlax",
                    type: "Normal",
                    createdAt: DateTime.UtcNow,
                    updatedAt: null
                )
            };

            foreach (var pokemon in demoPokemons)
            {
                _pokemonRepository.Add(pokemon);
            }

            _logger.LogInformation($"Seeded {demoPokemons.Count} demo Pokémon successfully");
        }
    }
}
