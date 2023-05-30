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
    
    public async Task<IEnumerable<VideoGameExam>> GetUserExams(string userId)
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
    S.subject_name AS SubjectName,
    (SELECT user_name from Users WHERE Users.user_id = S.teacher_id) AS TeacherName,
    (SELECT COUNT(*) FROM Questions WHERE Questions.exam_id = Exams.exam_id) AS QuestionCount
FROM
    Exams
        INNER JOIN Subjects S ON Exams.subject_id = S.subject_id
        INNER JOIN Users T ON S.teacher_id = T.user_id
        INNER JOIN Courses ON S.course_id = Courses.course_id
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
}