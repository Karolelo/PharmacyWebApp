using PrescriptionApp.Models;

namespace PrescriptionApp.Service;

public interface IUserService
{
    Task<int> RegisterUser(RegisterRequest request);
    Task<int> Login(LoginRequest loginRequest);
}