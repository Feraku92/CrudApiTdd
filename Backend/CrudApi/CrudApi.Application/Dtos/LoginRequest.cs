using System.Text.Json.Serialization;

namespace CrudApi.Application.Dtos
{
    public record LoginRequest(
        [property: JsonPropertyName("userName")] string UserName,
        [property: JsonPropertyName("password")] string Password
    );
}
