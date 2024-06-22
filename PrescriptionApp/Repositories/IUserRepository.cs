using GakkoHorizontalSlice.Model;
using PrescriptionApp.Models;

namespace PrescriptionApp.Repositories;

public interface IUserRepository
{
    Task<int> RegisterUser(User user);
    Task<bool>? IsLoginPossible(LoginRequest loginRequest);

    Task<string> UpdateRefreshToken(String login);

    Task<User> GetUserWithRefreshToken(RefreshTokenRequest refreshTokenRequest);
}