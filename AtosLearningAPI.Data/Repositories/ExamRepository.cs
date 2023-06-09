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

    public async Task<bool> SubmitExam(int studentId, int[] answersIds, int examId, float score, DateTime endDateTime)
    {
        var db = GetConnection();
        
        db.Open();
        var transaction = await db.BeginTransactionAsync();
        
        try
        {
            foreach (var answerId in answersIds)
            {
                var command = "INSERT INTO Submitted_Answers (user_id, answer_id, exam_id) VALUES (@studentId, @answerId, @examId)";
                await db.ExecuteAsync(command, new {studentId, answerId, examId}, transaction);
            }
            
            var command1 = "INSERT INTO Exam_Submissions (user_id, exam_id, end_date_time, exam_score) VALUES (@studentId, @examId,  @endDateTime, @score)";
            var command2 = "UPDATE Users SET user_total_score = user_total_score + @score WHERE user_id = @studentId";
            await db.ExecuteAsync(command1, new {studentId, examId, endDateTime, score}, transaction);
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

    public async Task<bool> DeleteExamSubmission(int studentId, int examId)
    {
        var db = GetConnection();
        
        db.Open();
        var transaction = await db.BeginTransactionAsync();

        try
        {
            var command1 = "DELETE FROM Submitted_Answers WHERE user_id = @studentId AND exam_id = @examId";
            var command2 = "DELETE FROM Exam_Submissions WHERE user_id = @studentId AND exam_id = @examId";
            
            await db.ExecuteAsync(command1, new {studentId, examId}, transaction);
            await db.ExecuteAsync(command2, new {studentId, examId}, transaction);
            
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