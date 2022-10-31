using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;

namespace AppCitas.Service.Interfaces;

public interface ILikesRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
    Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
    Task<AppUser> GetUserWithLikes(int userId);
}
