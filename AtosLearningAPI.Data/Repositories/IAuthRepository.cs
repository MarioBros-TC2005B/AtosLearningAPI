using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface IAuthRepository
{
    Task<Student> Login(string username, string password);
}