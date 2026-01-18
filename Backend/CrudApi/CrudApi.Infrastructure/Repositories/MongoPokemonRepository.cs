using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;
using CrudApi.Domain.Exceptions;
using MongoDB.Driver;

namespace CrudApi.Infrastructure.Repositories
{
    public class MongoPokemonRepository : IPokemonRepsitory
    {
        private readonly IMongoCollection<Pokemon> _pokemon;
        public MongoPokemonRepository(IMongoDatabase database)
        {
            _pokemon = database.GetCollection<Pokemon>("Pokemons");
        }
        public void Add(Pokemon pokemon)
        {
            var exists = _pokemon.Find(p => p.PokedexId == pokemon.PokedexId).Any();
            if (exists)
            {
                throw new DuplicatePokedexIdException(pokemon.PokedexId);
            }

            pokemon.CreatedAt = DateTime.UtcNow;
            _pokemon.InsertOne(pokemon);

        }

        public List<Pokemon> GetAll()
        {
            return _pokemon.Find(_ => true).ToList();
        }

        public Pokemon GetByNameOrNumber(string? name, int? number)
        {
            return _pokemon
                .Find(x =>
                    (!string.IsNullOrEmpty(name) && x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ||
                    (number.HasValue && x.PokedexId == number.Value)
                )
                .FirstOrDefault();
        }

        public Pokemon Update(string id, Pokemon updatedPokemon)
        {
            var filter = Builders<Pokemon>.Filter.Eq(x => x.Id, Guid.Parse(id));
            var update = Builders<Pokemon>.Update
                .Set(x => x.PokedexId, updatedPokemon.PokedexId)
                .Set(x => x.Name, updatedPokemon.Name)
                .Set(x => x.Type, updatedPokemon.Type)
                .Set(x => x.UpdatedAt, updatedPokemon.UpdatedAt);
            var options = new FindOneAndUpdateOptions<Pokemon>
            {
                ReturnDocument = ReturnDocument.After
            };
            return _pokemon.FindOneAndUpdate(filter, update, options);
        }

        public bool Delete(string id)
        {
            var result = _pokemon.DeleteOne(x => x.Id == Guid.Parse(id));
            return result.DeletedCount > 0;
        }
    }
}
