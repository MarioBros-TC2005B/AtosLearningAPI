using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories;

public class ExamRepository : IExamRepository
{
    private readonly MySQLConfiguration _mySqlConfiguration;
    
    public ExamRepository(MySQLConfiguration mySqlConfiguration)
    {
        _mySqlConfiguration = mySqlConfiguration;
    }
    
    protected MySqlConnection GetConnection() => new MySqlConnection(_mySqlConfiguration.ConnectionString);

    public async Task<bool> SubmitExam(int studentId, int[] answersIds, int examId, float score, DateTime startDateTime)
    {
        var db = GetConnection();
        
        db.Open();
        var transaction = await db.BeginTransactionAsync();
        
        try
        {
            foreach (var answerId in answersIds)
            {
                var command = "INSERT INTO Submitted_Answers (user_id, answer_id) VALUES (@studentId, @answerId)";
                await db.ExecuteAsync(command, new {studentId, answerId}, transaction);
            }
            
            var command1 = "INSERT INTO Exam_Submissions (user_id, exam_id, start_time, exam_score) VALUES (@studentId, @examId,  @startDateTime, @score)";
            var command2 = "UPDATE Users SET user_total_score = user_total_score + @score WHERE user_id = @studentId";
            await db.ExecuteAsync(command1, new {studentId, examId, startDateTime, score}, transaction);
            await db.ExecuteAsync(command2, new {score, studentId}, transaction);

            await transaction.CommitAsync();
            return true;

        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }
    }
}