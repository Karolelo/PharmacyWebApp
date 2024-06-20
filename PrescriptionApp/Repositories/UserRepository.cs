using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GakkoHorizontalSlice.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrescriptionApp.Models;

namespace PrescriptionApp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Context.Context _context;

    public UserRepository(Context.Context context)
    {
        _context = context;
    }

    public async Task<int> RegisterUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task<int> Login(LoginRequest loginRequest)
    {
        User user = await _context.Users.Where(e => e.Login == loginRequest.Login).FirstAsync();
        
        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            throw new UnauthorizedAccessException();
        }
        
        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, "pgago"),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
        };
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );
        
    }
}