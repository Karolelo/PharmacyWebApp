using PrescriptionApp.Models;

namespace PrescriptionApp.Repositories;

public interface IUserRepository
{
    Task<int> RegisterUser(User user);
    Task<int> Login(LoginRequest loginRequest);
}