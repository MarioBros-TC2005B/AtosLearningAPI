using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories.QuestionStat;

public interface IQuestionStatRepository
{
    Task<IEnumerable<Model.QuestionStat>> GetQuestionStatsByExam(int examId);
    
    Task<IEnumerable<User>> GetUsersWithPendingExam(int examId);
    
    Task<IEnumerable<QuestionUserStat>> GetQuestionUserStats(int examId, int userId);
    
    Task<IEnumerable<ExamSubmission>> GetSubmissionsByExam(int examId);
    
    Task<ExamSubmission> GetExamSubmissionByUser(int examId, int userId);
}