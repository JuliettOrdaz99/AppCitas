using AppCitas.Service.Data;
using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AppCitas.Service.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
            return BadRequest("Username is already taken!");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid username or password");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid username or password");
        }

        return user;
    }

    #region Private methods

    private async Task<bool> UserExists(string username)
    {
        return await
            _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

    #endregion
}
