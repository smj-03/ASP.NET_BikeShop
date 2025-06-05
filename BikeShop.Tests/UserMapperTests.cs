namespace BikeShop.Tests.Mappers
{
    using BikeShop.Mappers;
    using BikeShop.Models;
    using Xunit;

    public class UserMapperTests
    {
        private readonly UserMapper mapper;

        // Konstruktor działa jak [SetUp] w NUnit
        public UserMapperTests()
        {
            this.mapper = new UserMapper();
        }

        [Fact]
        public void Map_RegisterViewModel_To_ApplicationUser_Should_Map_Correctly()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "test@example.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                PhoneNumber = "123456789",
                Password = "Haslo123!",
            };

            // Act
            var user = this.mapper.Map(model);

            // Assert
            Assert.Equal(model.Email, user.Email);
            Assert.Equal(model.FirstName, user.FirstName);
            Assert.Equal(model.LastName, user.LastName);
            Assert.Equal(model.PhoneNumber, user.PhoneNumber);
        }

        [Fact]
        public void Map_ApplicationUser_To_UserDto_Should_Map_Correctly()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Email = "test@example.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                PhoneNumber = "123456789",
            };

            // Act
            var dto = this.mapper.Map(user);

            // Assert
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.FirstName, dto.FirstName);
            Assert.Equal(user.LastName, dto.LastName);
            Assert.Equal(user.PhoneNumber, dto.PhoneNumber);
        }
    }
}