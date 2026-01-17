using CrudApi.Domain.Entities;

namespace CrudApi.Application.Interfaces
{
    public interface IUserService
    {
        User RegisterUser(string username, string email, string password);
        User AuthenticateUser(string email, string password);
    }
}
