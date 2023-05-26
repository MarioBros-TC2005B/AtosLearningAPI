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
    
    public async Task<User> Login(string username, string password)
    {
        var db = DbConnection();
        
        
        var emailCommand = "SELECT U.user_id Id, user_name Name, user_email Email, user_nickname Nickname, character_id CharacterId, user_image Image, user_total_score TotalScore FROM Users U INNER JOIN User_Auth UA on U.user_id = UA.user_id WHERE LOWER(user_email) = LOWER(@username) AND user_password = @password";
        var nicknameCommand = "SELECT U.user_id Id, user_name Name, user_email Email, user_nickname Nickname, character_id CharacterId, user_image Image, user_total_score TotalScore FROM Users U INNER JOIN User_Auth UA on U.user_id = UA.user_id WHERE user_nickname = @username AND user_password = @password";

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
            
            return user;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}