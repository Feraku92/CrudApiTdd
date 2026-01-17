using CrudApi.Domain.Entities;

namespace CrudApi.Application.Interfaces
{
    public interface IUserRepository
    {
        User GetByEmail(string email);
        void Add(User user);
    }
}
