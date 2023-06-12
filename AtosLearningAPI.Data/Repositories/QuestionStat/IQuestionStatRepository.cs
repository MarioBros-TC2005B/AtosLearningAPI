namespace AtosLearningAPI.Data.Repositories.QuestionStat;

public interface IQuestionStatRepository
{
    Task<IEnumerable<Model.QuestionStat>> GetQuestionStatsByExam(int examId);
}