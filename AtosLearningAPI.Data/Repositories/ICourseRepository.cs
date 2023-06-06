using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface ICourseRepository
{
    Task<bool> JoinCourse(string studentId, string code);
}