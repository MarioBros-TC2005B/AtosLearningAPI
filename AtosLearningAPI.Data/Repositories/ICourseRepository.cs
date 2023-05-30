using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface ICourseRepository
{
    public Task<IEnumerable<Course>> GetCourses();
    public Task<Course> GetCourse(string id);
    public Task<Course> CreateCourse(Course course);
    public Task UpdateCourse(Course course);
    public Task DeleteCourse(string id);
}