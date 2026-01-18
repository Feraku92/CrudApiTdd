using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using CrudApi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace CrudApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;
        public UserService(IUserRepository userRepository, string jwtSecret) 
        {
            _userRepository = userRepository;
            _jwtSecret = jwtSecret;
        }

        public User RegisterUser(RegisterUserRequest userRequest)
        {
            if (_userRepository.GetByUserName(userRequest.UserName) != null)
                throw new Exception("User exists");
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            var user = new User(userRequest.UserName, userRequest.Email, hashedPassword);
            _userRepository.Add(user);
            return user;
        }

        public string AuthenticateUser(string username, string password)
        {
            var user = _userRepository.GetByUserName(username)
                       ?? throw new UnauthorizedAccessException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new UnauthorizedAccessException("Invalid password");

            // Generar JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
