using CrudApi.Application.Dtos;
using CrudApi.Domain.Entities;

namespace CrudApi.Application.Interfaces
{
    public interface IUserService
    {
        User RegisterUser(RegisterUserRequest request);
        string AuthenticateUser(string email, string password);
    }
}
