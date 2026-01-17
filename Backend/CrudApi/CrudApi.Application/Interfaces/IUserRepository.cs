using CrudApi.Domain.Entities;

namespace CrudApi.Application.Interfaces
{
    public interface IUserRepository
    {
        User GetByUserName(string userName);
        void Add(User user);
    }
}
