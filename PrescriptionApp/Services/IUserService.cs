using GakkoHorizontalSlice.Model;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;

namespace PrescriptionApp.Service;

public interface IUserService
{
    Task<int> RegisterUser(RegisterRequest request);
    Task<TokenResponse> Login(LoginRequest loginRequest);

    Task<TokenResponse> Refresh(RefreshTokenRequest refreshTokenRequest);
}