using System.Text.Json.Serialization;

namespace CrudApi.Application.Dtos
{
    public record PokemonRequest(
        [property: JsonPropertyName("pokedexId")] int PokedexId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("createdAt")] DateTime CreatedAt
        );
}