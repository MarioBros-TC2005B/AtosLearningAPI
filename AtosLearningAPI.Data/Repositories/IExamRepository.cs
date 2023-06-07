using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface IExamRepository
{
    Task<bool> SubmitExam(int studentId, int[] answersIds, int examId, float score, DateTime startDateTime);
    Task<bool> DeleteExamSubmission(int studentId, int examId);
}