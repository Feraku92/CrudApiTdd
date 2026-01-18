
using System.Text.Json.Serialization;

namespace CrudApi.Application.Dtos
{
    public record RegisterUserRequest(
        [property: JsonPropertyName("userName")] string UserName,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("password")] string Password
        );
}
