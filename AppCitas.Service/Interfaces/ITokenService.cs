using AppCitas.Service.Entities;

namespace AppCitas.Service.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
