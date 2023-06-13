using System.Data;
using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories;

public class SubjectRepository : ISubjectRepository
{
    
    private readonly MySQLConfiguration _mySQLConfiguration;
    
    public SubjectRepository(MySQLConfiguration mySQLConfiguration)
    {
        _mySQLConfiguration = mySQLConfiguration;
    }
    
    protected MySqlConnection GetConnection() => new MySqlConnection(_mySQLConfiguration.ConnectionString);
    
    public async Task<IEnumerable<Subject>> GetAllSubjects()
    {
        var db = GetConnection();
        var command = @"SELECT
 subject_id Id, 
 subject_name Name,
 course_id CourseId 
FROM Subjects";

        try
        {
            db.Open();
            var subjects = await db.QueryAsync<Subject>(command);
            return subjects.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<Subject> GetSubjectById(int id)
    {
        var db = GetConnection();

        var command = @"SELECT
 subject_id Id, 
 subject_name Name,
 course_id CourseId 
FROM Subjects WHERE subject_id = @id";
        
        try
        {
            db.Open();
            var result = await db.QueryAsync<Subject>(command, new {id});
            var subject = result.FirstOrDefault();
            if (subject == null)
                throw new Exception("Subject not found");
            
            return subject;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> AddSubject(Subject subject)
    {
        var db = GetConnection();

        var command = "INSERT INTO Subjects (subject_name, subject_description, course_id) VALUES (@name, @description, @courseId)";
        
        try
        {
            db.Open();
            await db.ExecuteAsync(command, new {name = subject.Name, description = subject.Description, courseId = subject.CourseId});
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<bool> UpdateSubject(Subject subject)
    {
        var db = GetConnection();
        
        var command = "UPDATE Subjects SET subject_name = @name, course_id = @courseId WHERE subject_id = @id";

        try
        {
            db.Open();
            await db.ExecuteAsync(command, new {name = subject.Name, courseId = subject.CourseId, id = subject.Id});
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteSubject(int id)
    {
        var db = GetConnection();
        
        var command = "DELETE FROM Subjects WHERE subject_id = @id";
        
        try
        {
            db.Open();
            var result = await db.ExecuteAsync(command, new {id});
            return result > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Subject>> GetTeacherSubjects(string teacherId)
    {
        var db = GetConnection();
        var command = "SELECT S.subject_id Id, S.subject_name Name, S.subject_description Description, S.course_id CourseId FROM Subjects S INNER JOIN Courses ON S.course_id = Courses.course_id WHERE Courses.teacher_id = @teacherId";
        
        try
        {
            db.Open();
            var subjects = await db.QueryAsync<Subject>(command, new {teacherId});
            var subjectList = subjects.ToList();
            if (subjectList.Count == 0)
                throw new Exception("No subjects found for this teacher");
            
            return subjectList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<IEnumerable<Subject>> GetStudentSubjects(string studentId)
    {
        var db = GetConnection();
        var command = @"SELECT
 S.subject_id Id,
 S.subject_name Name,
 S.subject_description Description,
 S.course_id CourseId
FROM Subjects S 
        INNER JOIN Course_Users CU
            ON S.course_id = CU.course_id 
WHERE CU.user_id = @studentId";
        
        try
        {
            db.Open();
            var subjects = await db.QueryAsync<Subject>(command, new {studentId});
            var subjectList = subjects.ToList();
            if (subjectList.Count == 0)
                throw new Exception("No subjects found for this student");
            
            return subjectList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    

}