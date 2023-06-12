using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories.QuestionStat;

public class QuestionStatRepository : IQuestionStatRepository
{
    private readonly MySQLConfiguration _mySqlConfiguration;
    
    public QuestionStatRepository(MySQLConfiguration mySqlConfiguration)
    {
        _mySqlConfiguration = mySqlConfiguration;
    }
    
    protected MySqlConnection GetConnection() => new MySqlConnection(_mySqlConfiguration.ConnectionString);
    
    public async Task<IEnumerable<Model.QuestionStat>> GetQuestionStatsByExam(int examId)
    {
        var db = GetConnection();
        
        db.Open();
        try
        {
            var command = @"
SELECT
    Q.question_id AS QuestionId,
    Q.question_title AS Question,
    Q.exam_id AS ExamId,
    COUNT(DISTINCT CASE WHEN A.is_correct = 1 THEN SA.user_id END) AS CorrectAnswers,
    COUNT(DISTINCT CASE WHEN A.is_correct = 0 THEN SA.user_id END) AS IncorrectAnswers
FROM
    Questions AS Q
        JOIN
    Answers AS A ON Q.question_id = A.question_id
        LEFT JOIN
    Submitted_Answers AS SA ON A.answer_id = SA.answer_id
WHERE
    Q.exam_id = @examId
GROUP BY
    Q.question_id";
            
            var questionStats = await db.QueryAsync<Model.QuestionStat>(command, new {examId});
            return questionStats;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}