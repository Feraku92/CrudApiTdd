using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;

namespace CrudApi.Application.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepsitory _pokemonRepsitory;
        public PokemonService(IPokemonRepsitory pokemonRepsitory) 
        {
            _pokemonRepsitory = pokemonRepsitory;
        }

        public Pokemon CreatePokemon(PokemonRequest requestPokemon)
        {
            var pokemon = new Pokemon(requestPokemon.PokedexId, requestPokemon.Name, requestPokemon.Type, DateTime.UtcNow, null);
            _pokemonRepsitory.Add(pokemon);
            return pokemon;
        }

        public bool DeletePokemon(string id)
        {
            return _pokemonRepsitory.Delete(id);
        }

        public List<Pokemon> GetAllPokemons()
        {
            return _pokemonRepsitory.GetAll();
        }

        public Pokemon GetByNameOrNumber(string? name, int? number)
        {
            return _pokemonRepsitory.GetByNameOrNumber(name, number);
        }

        public Pokemon UpdatePokemon(string id, PokemonRequest requestPokemon)
        {
            var updatedPokemon = new Pokemon(requestPokemon.PokedexId, requestPokemon.Name, requestPokemon.Type, requestPokemon.CreatedAt, DateTime.UtcNow);
            return _pokemonRepsitory.Update(id, updatedPokemon);
        }
    }
}
