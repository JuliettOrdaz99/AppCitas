using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;

namespace AppCitas.Service.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<bool> SaveAllAsync();
    void Update(AppUser user);

    Task<MemberDto> GetMemberAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
}
