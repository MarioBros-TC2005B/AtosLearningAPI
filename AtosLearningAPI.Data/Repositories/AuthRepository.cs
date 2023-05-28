using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly MySQLConfiguration _connectionString; 
    
    public AuthRepository(MySQLConfiguration connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected MySqlConnection DbConnection()
    {
        return new MySqlConnection(_connectionString.ConnectionString); 
    }
    
    public async Task<Student> Login(string username, string password)
    {
        var db = DbConnection();
        
        
        var emailTeacherCommand = "SELECT T.teacher_id Id, teacher_name Name, teacher_email Email FROM Teachers T INNER JOIN User_Auth UA on T.teacher_id = UA.user_id WHERE LOWER(teacher_email) = LOWER(@username) AND user_password = @password";
        var emailStudentCommand = "SELECT U.student_id Id, student_name Name, student_email Email, student_nickname Nickname, character_id CharacterId, student_image Image, student_total_score TotalScore FROM Students U INNER JOIN User_Auth UA on U.student_id = UA.user_id WHERE LOWER(student_email) = LOWER(@username) AND user_password = @password";
        var nicknameStudentCommand = "SELECT U.student_id Id, student_name Name, student_email Email, student_nickname Nickname, character_id CharacterId, student_image Image, student_total_score TotalScore FROM Students U INNER JOIN User_Auth UA on U.student_id = UA.user_id WHERE student_nickname = @username AND user_password = @password";

        try
        {
            db.Open();
            var result = await db.QueryAsync<Student>(emailStudentCommand, new {username, password});
            var user = result.FirstOrDefault();
            if (user == null)
            {
                result = await db.QueryAsync<Student>(nicknameStudentCommand, new {username, password});
                user = result.FirstOrDefault();
                if (user == null)
                    throw new Exception("Correo/Usuario o contraseña incorrectos");
            }
            
            return user;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}