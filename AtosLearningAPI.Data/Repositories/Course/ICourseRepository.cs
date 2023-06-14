using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface ICourseRepository
{
    Task<Course> JoinCourse(int studentId, string code);
}