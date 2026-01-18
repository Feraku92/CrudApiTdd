using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrudApi.Domain.Entities
{
    public class Pokemon
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int PokedexId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Pokemon(int pokedexid, string name, string type, DateTime createdAt, DateTime? updatedAt)
        {
            if (pokedexid <= 0)
                throw new ArgumentException("PokedexId must be greater than 0");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty");

            PokedexId = pokedexid;
            Name = name;
            Type = type;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
