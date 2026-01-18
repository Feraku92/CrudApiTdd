using CrudApi.Application.Dtos;
using CrudApi.Domain.Entities;

namespace CrudApi.Application.Interfaces
{
    public interface IPokemonService
    {
        List<Pokemon> GetAllPokemons();
        Pokemon GetByNameOrNumber(string? name, int? number);
        Pokemon CreatePokemon(PokemonRequest request);
        Pokemon UpdatePokemon(string id, PokemonRequest request);
        bool DeletePokemon(string id);
    }
}
