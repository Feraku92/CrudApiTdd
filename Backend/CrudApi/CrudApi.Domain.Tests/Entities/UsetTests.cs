using CrudApi.Domain.Entities;
using Xunit;

namespace CrudApi.Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_WithValidData_ShouldSetProperties()
        {
            // Arrange
            var username = "fernando";
            var email = "fernando@test.com";
            var password = "123456";

            // Act
            var user = new User(username, email, password);

            // Assert
            Assert.Equal(username, user.UserName);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
        }

        [Fact]
        public void CreateUser_WithEmptyUsername_ShouldThrowException()
        {
            // Arrange
            var username = "";
            var email = "fernando@test.com";
            var password = "123456";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new User(username, email, password));
        }

        [Fact]
        public void CreateUser_WithInvalidEmail_ShouldThrowException()
        {
            // Arrange
            var username = "fernando";
            var email = "invalid-email";
            var password = "123456";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new User(username, email, password));
        }
    }
}
