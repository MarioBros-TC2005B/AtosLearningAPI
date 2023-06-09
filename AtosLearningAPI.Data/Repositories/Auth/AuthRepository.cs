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

    public async Task<User> Register(User user, string password)
    {
        var db = DbConnection();

        var command = "INSERT INTO Users (user_name, user_surname, user_email, user_nickname, character_id, user_image, user_total_score, is_teacher) VALUES (@Name, @Surname, @Email, @Nickname, @CharacterId, @Image, @TotalScore, @IsTeacher)";
        var authCommand = "INSERT INTO User_Auth (user_id, user_password) VALUES (@Id, @Password)";
        
        try
        {
            db.Open();
            var result = await db.ExecuteAsync(command, user);
            if (result == 0)
                throw new Exception("Error al crear el usuario");
            
            var userId = await db.QueryAsync<int>("SELECT LAST_INSERT_ID()");
            user.Id = userId.FirstOrDefault();
            
            var authResult = await db.ExecuteAsync(authCommand, new {Id = user.Id, Password = password});
            if (authResult == 0)
                throw new Exception("Error al crear el usuario");

            if (user.IsTeacher)
            {
                // Create new course
                var courseCommand = "INSERT INTO Courses (course_name, teacher_id, course_code) VALUES (@Name, @TeacherId, @Code)";
                // Generate 5 character alphanumeric course code
                var courseCode = Guid.NewGuid().ToString().Substring(0, 5);
                var courseResult = await db.ExecuteAsync(courseCommand, new {Name = "Curso de " + user.Name, TeacherId = user.Id, Code = courseCode});
                if (courseResult == 0)
                    throw new Exception("Error al crear el curso");
            }

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<User> Login(string username, string password)
    {
        var db = DbConnection();
        
        
        var emailCommand = @"SELECT 
    U.user_id Id, 
    user_name Name,
    user_surname Surname, 
    user_email Email, 
    user_nickname Nickname,
    character_id CharacterId,
    user_image Image, 
    user_total_score TotalScore,
    is_teacher IsTeacher
FROM Users U
    INNER JOIN 
    User_Auth UA on U.user_id = UA.user_id 
WHERE 
    LOWER(user_email) = LOWER(@username)
  AND 
    user_password = @password";
        var nicknameCommand = "SELECT U.user_id Id, user_name Name, user_surname Surname, user_email Email, user_nickname Nickname, character_id CharacterId, user_image Image, user_total_score TotalScore, is_teacher IsTeacher FROM Users U INNER JOIN User_Auth UA on U.user_id = UA.user_id WHERE user_nickname = @username AND user_password = @password";
        
        try
        {
            db.Open();
            var result = await db.QueryAsync<User>(emailCommand, new {username, password});
            var user = result.FirstOrDefault();
            if (user == null)
            {
                result = await db.QueryAsync<User>(nicknameCommand, new {username, password});
                user = result.FirstOrDefault();
                if (user == null)
                    throw new Exception("Correo/Usuario o contraseña incorrectos");
            }

            if (user.IsTeacher)
            {
                var courseCommand = "SELECT course_id Id, course_name Name, teacher_id TeacherId, course_code Code FROM Courses WHERE teacher_id = @Id";
                var courseResult = await db.QueryAsync<Course>(courseCommand, new {Id = user.Id});
                user.Course = courseResult.FirstOrDefault();
            }
            else
            {
                var courseCommand = "SELECT C.course_id Id, course_name Name, teacher_id TeacherId, course_code Code FROM Courses C INNER JOIN Course_Users CU on C.course_id = CU.course_id WHERE CU.user_id = @Id";
                var courseResult = await db.QueryAsync<Course>(courseCommand, new {Id = user.Id});
                user.Course = courseResult.FirstOrDefault();
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
