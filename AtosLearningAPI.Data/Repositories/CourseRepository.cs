using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly MySQLConfiguration _mySQLConfiguration;
    
    public CourseRepository(MySQLConfiguration mySQLConfiguration)
    {
        _mySQLConfiguration = mySQLConfiguration;
    }
    
    protected MySqlConnection GetConnection() => new MySqlConnection(_mySQLConfiguration.ConnectionString);
    
    public async Task<bool> JoinCourse(string studentId, string code)
    {
        
        // Check if course and student exist
        var db = GetConnection();
        var command = "SELECT course_id Id FROM Courses WHERE course_code = @code";
        
        try
        {
            db.Open();
            var courseId = await db.QueryFirstOrDefaultAsync<int>(command, new {code});
            if (courseId == 0)
                throw new Exception("Course not found");
            
            command = "SELECT user_id Id FROM Users WHERE user_id = @studentId";
            var student = await db.QueryFirstOrDefaultAsync<string>(command, new {studentId});
            if (student == null)
                throw new Exception("Student not found");
            
            // Check if student is already in course
            command = "SELECT user_id Id FROM Course_Users WHERE user_id = @studentId AND course_id = @courseId";
            var studentSubject = await db.QueryFirstOrDefaultAsync<string>(command, new {studentId, courseId});
            if (studentSubject != null)
                throw new Exception("Student already in subject");
            
            // Add student to course
            command = "INSERT INTO Course_Users (user_id, course_id) VALUES (@studentId, @courseId)";
            await db.ExecuteAsync(command, new {studentId, courseId});
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    
    }
}