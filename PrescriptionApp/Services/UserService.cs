using GakkoHorizontalSlice.Helpers;
using PrescriptionApp.Models;
using PrescriptionApp.Repositories;
using PrescriptionApp.Service;

namespace PrescriptionApp.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
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

    public async Task<int> Login(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }
}