using AtosLearningAPI.Model;
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
    
    public async Task<IEnumerable<User>> GetUsersWithPendingExam(int examId)
    {
        var db = GetConnection();
        
        db.Open();
        try
        {
            var command = @"
SELECT
    U.user_id AS Id,
    U.user_name AS Name,
    U.user_surname AS Surname,
    U.user_email AS Email,
    U.user_nickname AS Nickname,
    U.character_id AS CharacterId,
    U.user_image AS Image,
    U.user_total_score AS TotalScore
FROM
    Users AS U
        JOIN Course_Users AS CU ON U.user_id = CU.user_id
WHERE
        U.user_id NOT IN (
        SELECT DISTINCT
            ES.user_id
        FROM
            Exam_Submissions AS ES
        WHERE
                ES.exam_id = @examId
    )
  AND U.is_teacher = 0
  AND CU.course_id = (
    SELECT
        C.course_id
    FROM
        Exams AS E
            JOIN Subjects AS S ON E.subject_id = S.subject_id
            JOIN Courses AS C ON S.course_id = C.course_id
    WHERE
            E.exam_id = @examId
)
";
            var users = await db.QueryAsync<User>(command, new {examId});
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<QuestionUserStat>> GetQuestionUserStats(int examId, int userId)
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
    MAX(CASE WHEN A.is_correct = 1 AND SA.user_id = @userId THEN 1 ELSE 0 END) AS Correct
FROM
    Questions AS Q
        JOIN
    Answers AS A ON Q.question_id = A.question_id
        LEFT JOIN
    Submitted_Answers AS SA ON A.answer_id = SA.answer_id
WHERE
        Q.exam_id = @examId
GROUP BY
    Q.question_id
";
            
            var questionStats = await db.QueryAsync<QuestionUserStat>(command, new {examId, userId});
            return questionStats;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
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
WHERE
    ES.exam_id = @examId
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

    public async Task<ExamSubmission> GetExamSubmissionByUser(int examId, int userId)
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
WHERE
    ES.exam_id = @examId
    AND ES.user_id = @userId
";

            var result = await db.QueryAsync<ExamSubmission>(command, new { examId, userId });
            var examSubmission = result.FirstOrDefault();
            return examSubmission;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}