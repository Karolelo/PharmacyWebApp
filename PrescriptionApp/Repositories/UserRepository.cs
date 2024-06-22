using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GakkoHorizontalSlice.Helpers;
using GakkoHorizontalSlice.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrescriptionApp.Models;

namespace PrescriptionApp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Context.Context _context;
    private readonly IConfiguration _configuration;

    public UserRepository(Context.Context context,IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<int> RegisterUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }
    
    public async Task<bool>? IsLoginPossible(LoginRequest loginRequest)
    {
        User user = await _context.Users.Where(e => e.Login == loginRequest.Login).FirstAsync();
        
        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return false;
        }

        return true;
    }

    public async Task<string> UpdateRefreshToken(String login)
    {
        var user= await _context.Users.FirstAsync(e => e.Login == login);
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Today.AddDays(1);
        await _context.SaveChangesAsync();
        
        return user.RefreshToken;
    }

    public async Task<User> GetUserWithRefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var user = await _context.Users.FirstAsync(e => e.RefreshToken == refreshTokenRequest.RefreshToken);

        return user;
    }
}