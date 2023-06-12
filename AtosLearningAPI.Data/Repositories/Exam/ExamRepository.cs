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

    public async Task<IEnumerable<Exam>> GetExams()
    {
        var db = GetConnection();
        
        db.Open();

        try
        {
            var command = @"
SELECT 
    exam_id Id, 
    exam_title Title, 
    exam_description Description,
    due_date DueDate,
    exam_image ImageUrl,
    E.subject_id SubjectId
FROM Exams E 
    INNER JOIN Subjects S ON E.subject_id = S.subject_id
";
            
            var result = await db.QueryAsync<Exam>(command);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> AddExam(Exam exam, List<Question> questions)
    {
        var db = GetConnection();
        
        db.Open();
        var transaction = await db.BeginTransactionAsync();

        try
        {
            var examCommand = @"
INSERT INTO Exams
    (exam_title, subject_id, exam_description, due_date, exam_image)
VALUES
    (@Title, @SubjectId, @Description, @DueDate, @ImageUrl)
";
            var questionCommand = @"
INSERT INTO Questions
    (question_title, exam_id, time_limit)
VALUES
    (@Title, @ExamId, @TimeLimit)
";

            var answerCommand = @"
INSERT INTO Answers
    (answer_title, question_id, is_correct)
VALUES
    (@Title, @QuestionId, @IsCorrect)
";
            
            var examId = await db.ExecuteScalarAsync<int>(examCommand, exam, transaction);
            
            

            foreach (var question in questions)
            {
                var questionId = await db.ExecuteScalarAsync<int>(questionCommand,
                    new { question.Title, ExamId = examId, question.TimeLimit }, transaction);

                foreach (var answer in question.Answers)
                {
                    await db.ExecuteAsync(answerCommand,
                        new { answer.Text, QuestionId = questionId, answer.IsCorrect }, transaction);
                }

            }
            
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

    public async Task<bool> DeleteExam(int id)
    {
        var db = GetConnection();
        
        db.Open();
        var transaction = db.BeginTransaction();
        
        try
        {
            var command1 = "DELETE FROM Exams WHERE exam_id = @id";
            var command2 = "DELETE FROM Questions WHERE exam_id = @id";
            var command3 = "DELETE FROM Answers WHERE question_id IN (SELECT question_id FROM Questions WHERE exam_id = @id)";
            
            await db.ExecuteAsync(command1, new {id}, transaction);
            await db.ExecuteAsync(command2, new {id}, transaction);
            await db.ExecuteAsync(command3, new {id}, transaction);
            
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
    
    public async Task<IEnumerable<Exam>> GetExamsBySubject(int subjectId)
    {
        var db = GetConnection();
        
        db.Open();

        try
        {
            var command = @"
SELECT 
    exam_id Id, 
    exam_title Title, 
    exam_description Description,
    due_date DueDate,
    exam_image ImageUrl,
    E.subject_id SubjectId
FROM Exams E
    INNER JOIN Subjects S ON E.subject_id = S.subject_id
WHERE E.subject_id = @subjectId
";
            
            var result = await db.QueryAsync<Exam>(command, new {subjectId});
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Exam>> GetSubmittedExamsByStudent(int studentId)
    {
        var db = GetConnection();
        
        db.Open();

        try
        {
            var command = @"
SELECT 
    exam_id Id, 
    exam_title Title, 
    exam_description Description,
    due_date DueDate,
    exam_image ImageUrl,
    E.subject_id SubjectId
FROM Exams E
    INNER JOIN Subjects S ON E.subject_id = S.subject_id
WHERE E.exam_id IN (SELECT exam_id FROM Exam_Submissions WHERE user_id = @studentId)
";
            
            var result = await db.QueryAsync<Exam>(command, new {studentId});
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Exam>> GetPendingExamsByStudent(int studentId)
    {
        var db = GetConnection();
        
        db.Open();

        try
        {
            var command = @"
SELECT 
    exam_id Id, 
    exam_title Title, 
    exam_description Description,
    due_date DueDate,
    exam_image ImageUrl,
    E.subject_id SubjectId
FROM Exams E
    INNER JOIN Subjects S ON E.subject_id = S.subject_id
WHERE E.exam_id NOT IN (SELECT exam_id FROM Exam_Submissions WHERE user_id = @studentId)
";
            
            var result = await db.QueryAsync<Exam>(command, new {studentId});
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<IEnumerable<Exam>> GetExamsByCourse(int courseId)
    {
        var db = GetConnection();
        
        db.Open();

        try
        {
            var command = @"
SELECT 
    exam_id Id, 
    exam_title Title, 
    exam_description Description,
    due_date DueDate,
    exam_image ImageUrl,
    E.subject_id SubjectId
FROM Exams E
    INNER JOIN Subjects S ON E.subject_id = S.subject_id
WHERE E.subject_id IN (SELECT subject_id FROM Subjects WHERE course_id = @courseId)
";
            
            var result = await db.QueryAsync<Exam>(command, new {courseId});
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Exam>> GetActiveExamsByCourse(int courseId)
    {
        try
        {
            var result = await GetExamsByCourse(courseId);
            result = result.Where(e => e.DueDate > DateTime.Now);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Exam>> GetInactiveExamsByCourse(int courseId)
    {
        try
        {
            var result = await GetExamsByCourse(courseId);
            result = result.Where(e => e.DueDate < DateTime.Now);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}