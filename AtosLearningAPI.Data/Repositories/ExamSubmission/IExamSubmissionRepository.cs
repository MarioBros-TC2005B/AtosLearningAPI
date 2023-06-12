using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface IExamSubmissionRepository
{
    Task<IEnumerable<ExamSubmission>> GetSubmissionsByExam(int examId);
}