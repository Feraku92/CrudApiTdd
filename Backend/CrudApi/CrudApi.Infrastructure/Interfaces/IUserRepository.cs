
using CrudApi.Domain.Entities;

namespace CrudApi.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetByEmail(string email);
    }
}
