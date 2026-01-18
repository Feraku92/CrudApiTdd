using CrudApi.Domain.Entities;

namespace CrudApi.Application.Interfaces
{
    public interface IPokemonRepsitory
    {
        void Add(Pokemon pokemon);
        List<Pokemon> GetAll();
        Pokemon GetByNameOrNumber(string? name, int? number);
        Pokemon Update(string id, Pokemon updatedPokemon);
        bool Delete(string id);
    }
}
