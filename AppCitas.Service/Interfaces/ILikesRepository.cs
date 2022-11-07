using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Helpers;

namespace AppCitas.Service.Interfaces;

public interface ILikesRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    Task<AppUser> GetUserWithLikes(int userId);
}
