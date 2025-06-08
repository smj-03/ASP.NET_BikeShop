namespace BikeShop.Services;

public interface IRoleInitializer
{
    Task EnsureRolesExistAsync();
}