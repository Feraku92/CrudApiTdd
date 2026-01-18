using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;
using MongoDB.Driver;

namespace CrudApi.Infrastructure.Repositories
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public MongoUserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public void Add(User user)
        {
            _users.InsertOne(user);
        }

        public User GetByUserName(string userName)
        {
            return _users.Find(u => u.UserName == userName).FirstOrDefault();
        }
    }
}
