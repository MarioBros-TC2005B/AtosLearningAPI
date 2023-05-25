using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLConfiguration _connectionString; 
        public UserRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString); 
        }


        public async Task<bool> DeleteUser(User user)
        {
            var db = DbConnection();
            var command = "DELETE FROM Users WHERE user_id = @Id";

            try
            {
                db.Open();
                var result = await db.ExecuteAsync(command, new {user.Id});
                return result > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var db = DbConnection();
            
            var command = "SELECT user_id Id, user_name Name, user_email Email, user_password Password, user_nickname Nickname, character_id CharacterId, user_image Image, user_total_score TotalScore FROM Users";

            try
            {
                db.Open();
                var users = await db.QueryAsync<User>(command);
                return users.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<User> GetUserById(int id)
        {
            var db = DbConnection();
            
            var command = "SELECT user_id Id, user_name Name, user_email Email, user_password Password, user_nickname Nickname, character_id CharacterId, user_image Image, user_total_score TotalScore FROM Users WHERE user_id = @id";

            try
            {
                db.Open();
                var result = await db.QueryAsync<User>(command, new {id});
                var user = result.FirstOrDefault();
                if (user == null)
                    throw new Exception("User not found");
                
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            var db = DbConnection();

            var command =
                "UPDATE Users SET user_name = @name, user_nickname = @nickname, character_id = @characterId, user_image = @userImage, user_total_score = @totalScore WHERE user_id = @id";

            try
            {
                db.Open();
                var result = await db.ExecuteAsync(command, new
                {
                    name = user.Name,
                    nickname = user.Nickname,
                    characterId = user.CharacterId,
                    userImage = user.Image,
                    totalScore = user.TotalScore,
                    id = user.Id
                });
                return result > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> InsertUser(User user)
        {
            var db = DbConnection();
            
            var command =
                "INSERT INTO Users (user_name, user_email, user_password, user_nickname, character_id, user_image, user_total_score) VALUES (@name, @email, @password, @nickname, @characterId, @userImage, @totalScore)";

            try
            {
                db.Open();
                var result = await db.ExecuteAsync(command, new
                {
                    name = user.Name,
                    email = user.Email,
                    password = user.Password,
                    nickname = user.Nickname,
                    characterId = user.CharacterId,
                    userImage = user.Image,
                    totalScore = user.TotalScore
                });
                return result > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

    }
}
