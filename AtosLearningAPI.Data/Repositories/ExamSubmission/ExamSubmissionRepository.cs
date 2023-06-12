using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories;

public class ExamSubmissionRepository : IExamSubmissionRepository
{
    private readonly MySQLConfiguration _mySqlConfiguration;
    
    public ExamSubmissionRepository(MySQLConfiguration mySqlConfiguration)
    {
        _mySqlConfiguration = mySqlConfiguration;
    }
    
    protected MySqlConnection GetConnection() => new MySqlConnection(_mySqlConfiguration.ConnectionString);
    
    public async Task<IEnumerable<ExamSubmission>> GetSubmissionsByExam(int examId)
    {
        var db = GetConnection();

        db.Open();
        try
        {
            var command = @"
SELECT
    ES.exam_id ExamId,
    ES.user_id UserId,
    CONCAT(S.user_name, ' ', S.user_surname) UserName,
    ES.exam_score Score,
    ES.end_date_time Date
FROM Exam_Submissions ES
    INNER JOIN Users S ON ES.user_id = S.user_id
";

            var result = await db.QueryAsync<ExamSubmission>(command, new { examId });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}