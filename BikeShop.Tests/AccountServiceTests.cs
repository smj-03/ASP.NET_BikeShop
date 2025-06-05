using System.Threading.Tasks;
using BikeShop.Mappers;
using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class AccountServiceTests
{
    [Fact]
    public async Task RegisterAsync_Should_Assign_Client_Role_To_New_User()
    {
        // Arrange
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            null, null, null, null, null, null, null, null);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

        var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            userManagerMock.Object,
            httpContextAccessorMock.Object,
            userClaimsPrincipalFactoryMock.Object,
            null, null, null, null);

        // Używamy prawdziwej instancji UserMapper (nie mockujemy)
        var userMapper = new UserMapper();

        var accountService = new AccountService(userManagerMock.Object, signInManagerMock.Object, userMapper);

        var registerViewModel = new RegisterViewModel
        {
            Email = "test@example.com",
            Password = "Password123!",
        };

        // Mockowanie tworzenia użytkownika na sukces
        userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), registerViewModel.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Mockowanie dodawania do roli na sukces
        userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Client"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await accountService.RegisterAsync(registerViewModel);

        // Assert
        userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Client"), Times.Once);
    }
}
