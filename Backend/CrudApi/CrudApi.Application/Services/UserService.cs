using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;

namespace CrudApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository; 
        }

        public User RegisterUser(string username, string email, string password)
        {
            if (_userRepository.GetByEmail(email) != null)
                throw new Exception("User exists");

            var user = new User(username, email, password);

            _userRepository.Add(user);
            return user;
        }

        public User AuthenticateUser(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            if (user.Password != password)
                throw new UnauthorizedAccessException("Invalid password");

            return user;
        }
    }
}
