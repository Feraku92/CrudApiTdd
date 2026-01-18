
namespace CrudApi.Domain.Exceptions
{
    public class DuplicatePokedexIdException : Exception
    {
        public DuplicatePokedexIdException(int pokedexId)
            : base($"There is already a Pokémon with PokedexId {pokedexId}") { }
    }

}
