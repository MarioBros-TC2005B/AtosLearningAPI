using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface IAuthRepository
{
    Task<User> Login(string username, string password);
    Task<User> Register(User user, string password);
}