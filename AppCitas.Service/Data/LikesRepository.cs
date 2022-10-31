using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Interfaces;
using AutoMapper;

namespace AppCitas.Service.Data;

public class LikesRepository : ILikesRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public LikesRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> GetUserWithLikes(int userId)
    {
        throw new NotImplementedException();
    }
}
