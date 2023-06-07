using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories;

public class VideoGameExamRepository : IVideoGameExamRepository
{
    private readonly MySQLConfiguration _mySqlConfiguration; 
    
    public VideoGameExamRepository(MySQLConfiguration mySqlConfiguration)
    {
        _mySqlConfiguration = mySqlConfiguration;
    }
    
    protected MySqlConnection GetConnection() => new MySqlConnection(_mySqlConfiguration.ConnectionString);
    
    public async Task<IEnumerable<VideoGameExam>> GetUserExams(int userId)
    {
        var db = GetConnection();

        var cmd =
            @"
SELECT
    Exams.exam_id AS Id,
    Exams.exam_title AS Title,
    Exams.exam_description AS Description,
    Exams.due_date AS DueDate,
    Exams.subject_id AS SubjectId,
    Exams.exam_image as ImageUrl,
    S.subject_name AS SubjectName,
    (SELECT user_name from Users WHERE Users.user_id = Courses.teacher_id) AS TeacherName,
    (SELECT COUNT(*) FROM Questions WHERE Questions.exam_id = Exams.exam_id) AS QuestionsCount
FROM
    Exams
        INNER JOIN Subjects S ON Exams.subject_id = S.subject_id
        INNER JOIN Courses ON S.course_id = Courses.course_id
        INNER JOIN Users T ON Courses.teacher_id = T.user_id
        INNER JOIN Course_Users ON Courses.course_id = Course_Users.course_id
        INNER JOIN Users U ON Course_Users.user_id = U.user_id
WHERE
        U.user_id = @userId
";
        // Get exam question count from table Questions where exam_id = exam_id
        
        //var cmd = "SELECT exam_id Id, exam_title Title, exam_description Description, due_date DueDate, E.subject_id SubjectId, S.subject_name SubjectName, T.user_name TeacherName FROM Exams E INNER JOIN Subjects S ON E.subject_id = S.subject_id INNER JOIN Users T ON S.teacher_id = T.user_id  INNER JOIN Courses C on S.course_id = C.course_id INNER JOIN Users U on S.teacher_id = @userId";
        
        try
        {
            db.Open();
            var exams = await db.QueryAsync<VideoGameExam>(cmd, new {userId});
            return exams.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Question>> GetExamQuestions(int examId)
    {
        var db = GetConnection();

        try
        {
            db.Open();
            var cmd = @"
SELECT
    Q.question_id AS Id,
    Q.question_title AS Title,
    Q.exam_id AS ExamId,
    Q.time_limit AS TimeLimit
FROM Questions Q
WHERE Q.exam_id = @examId
";
            var questions = await db.QueryAsync<Question>(cmd, new {examId});
            foreach (var question in questions)
            {
                var cmd2 = @"
SELECT
    A.answer_id AS Id,
    A.answer_title AS Text,
    A.is_correct AS IsCorrect
FROM Answers A
WHERE A.question_id = @questionId
";
                var answers = await db.QueryAsync<Answer>(cmd2, new {questionId = question.Id});
                question.Answers = answers.ToArray();
            }

            return questions;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<VideoGameExam>> GetSubmittedExams(int userId)
    {
        var db = GetConnection();
        
        db.Open();

        var cmd = @"
SELECT
    Exams.exam_id AS Id,
    Exams.exam_title AS Title,
    Exams.exam_description AS Description,
    Exams.due_date AS DueDate,
    Exams.subject_id AS SubjectId,
    Exams.exam_image as ImageUrl,
    S.subject_name AS SubjectName,
    (SELECT user_name from Users WHERE Users.user_id = C.teacher_id) AS TeacherName,
    (SELECT COUNT(*) FROM Questions WHERE Questions.exam_id = Exams.exam_id) AS QuestionsCount
FROM
    Exams
INNER JOIN
    Subjects S ON Exams.subject_id = S.subject_id
INNER JOIN
        Courses C on S.course_id = C.course_id
INNER JOIN
    Exam_Submissions ES ON Exams.exam_id = ES.exam_id
WHERE
    ES.user_id = @userId
";
        
        try
        {
            var exams = await db.QueryAsync<VideoGameExam>(cmd, new {userId});
            return exams.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<IEnumerable<VideoGameExam>> GetPendingExams(int userId)
    {
        var db = GetConnection();
        
        db.Open();

        var cmd = @"
SELECT
    Exams.exam_id AS Id,
    Exams.exam_title AS Title,
    Exams.exam_description AS Description,
    Exams.due_date AS DueDate,
    Exams.subject_id AS SubjectId,
    Exams.exam_image as ImageUrl,
    S.subject_name AS SubjectName,
    (SELECT user_name from Users WHERE Users.user_id = C.teacher_id) AS TeacherName,
    (SELECT COUNT(*) FROM Questions WHERE Questions.exam_id = Exams.exam_id) AS QuestionsCount
FROM
    Exams
INNER JOIN
    Subjects S ON Exams.subject_id = S.subject_id
INNER JOIN
    Courses C on S.course_id = C.course_id
WHERE
    Exams.exam_id NOT IN (SELECT exam_id FROM Exam_Submissions WHERE user_id = @userId)
";
        
        try
        {
            var exams = await db.QueryAsync<VideoGameExam>(cmd, new {userId});
            return exams.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}