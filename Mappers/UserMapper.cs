using BikeShop.Models;
using Riok.Mapperly.Abstractions;

namespace BikeShop.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial ApplicationUser Map(RegisterViewModel source);
    public partial UserDto Map(ApplicationUser source);
}