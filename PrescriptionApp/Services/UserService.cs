using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using GakkoHorizontalSlice.Helpers;
using GakkoHorizontalSlice.Model;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.IdentityModel.Tokens;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;
using PrescriptionApp.Repositories;
using PrescriptionApp.Service;

namespace PrescriptionApp.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    private readonly IConfiguration _configuration;
    
    private readonly Claim[] userClaims = new[]
    {
        new Claim(ClaimTypes.Name, "pgago"),
        new Claim(ClaimTypes.Role, "user"),
        new Claim(ClaimTypes.Role, "admin")
    };
    public UserService(UserRepository repository,IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<int> RegisterUser(RegisterRequest request)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);


        var user = new User()
        {
            Email = request.Email,
            Login = request.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        return await _repository.RegisterUser(user);
    }

    public async Task<TokenResponse> Login(LoginRequest loginRequest)
    {
        if (!await _repository.IsLoginPossible(loginRequest))
        {
            throw new UnauthorizedAccessException();
        }
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        string refreshToken = await _repository.UpdateRefreshToken(loginRequest.Login);

        return new TokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    public async Task<TokenResponse> Refresh(RefreshTokenRequest refreshTokenRequest)
    {
        var user = await _repository.GetUserWithRefreshToken(refreshTokenRequest);
        if (user == null)
        {
            throw new SecurityException("Invalid refresh token");
        }   
        
        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        string refreshToken = await _repository.UpdateRefreshToken(user.Login);

        return new TokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
        };
    }
}