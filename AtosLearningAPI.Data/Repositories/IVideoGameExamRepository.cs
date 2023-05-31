using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface IVideoGameExamRepository
{
    Task<IEnumerable<VideoGameExam>> GetUserExams(int userId);
    Task<IEnumerable<Question>> GetExamQuestions(int examId);
}