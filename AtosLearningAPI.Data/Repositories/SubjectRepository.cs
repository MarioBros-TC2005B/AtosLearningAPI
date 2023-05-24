using System.Data;
using AtosLearningAPI.Model;
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
    
    public Task<IEnumerable<Subject>> GetAllSubjects()
    {
        var db = GetConnection();
        db.Open();

        MySqlCommand cmd = new MySqlCommand();

        cmd.Connection = db;
        //cmd.CommandType = CommandType.Text;
        cmd.CommandText = "SELECT * FROM Subjects";
        
        List<Subject> subjects = new List<Subject>();
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Subject subject = new Subject();
                subject.Id = Convert.ToInt32(reader["subject_id"]);
                subject.Name = reader["subject_name"].ToString();
                subject.TeacherId = Convert.ToInt32(reader["teacher_id"].ToString());
                subject.CourseId = Convert.ToInt32(reader["course_id"].ToString());
                subjects.Add(subject);
            }
        } 
        
        db.Dispose();
        
        return Task.FromResult<IEnumerable<Subject>>(subjects);

    }

    public Task<Subject> GetSubjectById(int id)
    {
        var db = GetConnection();

        try
        {
            db.Open();
        
            MySqlCommand cmd = new MySqlCommand();
        
        
            cmd.Connection = db;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Subjects WHERE subject_id = @id";
            cmd.Parameters.AddWithValue("@id", id);
        
            Subject subject = new Subject();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    subject.Id = Convert.ToInt32(reader["subject_id"]);
                    subject.Name = reader["subject_name"].ToString();
                    subject.TeacherId = Convert.ToInt32(reader["teacher_id"].ToString());
                    subject.CourseId = Convert.ToInt32(reader["course_id"].ToString());
                }
            }
            db.Dispose();
            return Task.FromResult(subject);
        } catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<bool> AddSubject(Subject subject)
    {
        var db = GetConnection();
        

        try
        { 
            db.Open();
        
            MySqlCommand cmd = new MySqlCommand();
        
            cmd.Connection = db;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Subjects (subject_name, teacher_id, course_id) VALUES (@name, @teacherId, @courseId)";
            cmd.Parameters.AddWithValue("@name", subject.Name);
            cmd.Parameters.AddWithValue("@teacherId", subject.TeacherId);
            cmd.Parameters.AddWithValue("@courseId", subject.CourseId);
            cmd.ExecuteNonQuery();
            db.Dispose();
        
            return Task.FromResult(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public Task<bool> UpdateSubject(Subject subject)
    {
        var db = GetConnection();

        try
        {
            db.Open();
        
            MySqlCommand cmd = new MySqlCommand();
        
            cmd.Connection = db;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Subjects SET subject_name = @name, teacher_id = @teacherId, course_id = @courseId WHERE subject_id = @id";
            cmd.Parameters.AddWithValue("@id", subject.Id);
            cmd.Parameters.AddWithValue("@name", subject.Name);
            cmd.Parameters.AddWithValue("@teacherId", subject.TeacherId);
            cmd.Parameters.AddWithValue("@courseId", subject.CourseId);
            cmd.ExecuteNonQuery();
            db.Dispose();
        
            return Task.FromResult(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<bool> DeleteSubject(int id)
    {
        var db = GetConnection();
        
        try
        {
            db.Open();
        
            MySqlCommand cmd = new MySqlCommand();
        
            cmd.Connection = db;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "DELETE FROM Subjects WHERE subject_id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            db.Dispose();
        
            return Task.FromResult(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}