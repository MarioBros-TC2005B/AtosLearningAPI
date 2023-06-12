using AtosLearningAPI.Model;

namespace AtosLearningAPI.Data.Repositories;

public interface IExamRepository
{
    Task<bool> SubmitExam(int studentId, int[] answersIds, int examId, float score, DateTime endDateTime);
    Task<bool> DeleteExamSubmission(int studentId, int examId);
    Task<IEnumerable<Exam>> GetExams();
    Task<bool> AddExam(Exam exam, List<Question> questions);
    Task<bool> DeleteExam(int id);
    Task<IEnumerable<Exam>> GetExamsBySubject(int subjectId);
    Task<IEnumerable<Exam>> GetSubmittedExamsByStudent(int studentId);
    Task<IEnumerable<Exam>> GetPendingExamsByStudent(int studentId);
    
    Task<IEnumerable<Exam>> GetExamsByCourse(int courseId);
    Task<IEnumerable<Exam>> GetActiveExamsByCourse(int courseId);
    Task<IEnumerable<Exam>> GetInactiveExamsByCourse(int courseId);

}