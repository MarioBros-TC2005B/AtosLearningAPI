using AtosLearningAPI.Model;
using Dapper;
using MySql.Data.MySqlClient;

namespace AtosLearningAPI.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MySQLConfiguration _connectionString; 
        public StudentRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString); 
        }


        public async Task<bool> DeleteUser(Student student)
        {
            var db = DbConnection();
            var command = "DELETE FROM Students WHERE student_id = @Id";

            try
            {
                db.Open();
                var result = await db.ExecuteAsync(command, new {student.Id});
                return result > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Student>> GetAllUsers()
        {
            var db = DbConnection();
            
            var command = "SELECT student_id Id, student_name Name, student_email Email, student_nickname Nickname, character_id CharacterId, student_image Image, student_total_score TotalScore FROM Students";

            try
            {
                db.Open();
                var users = await db.QueryAsync<Student>(command);
                return users.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Student> GetUserById(int id)
        {
            var db = DbConnection();
            
            var command = "SELECT student_id Id, student_name Name, student_email Email, student_nickname Nickname, character_id CharacterId, student_image Image, student_total_score TotalScore FROM Students WHERE student_id = @id";

            try
            {
                db.Open();
                var result = await db.QueryAsync<Student>(command, new {id});
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

        public async Task<bool> UpdateUser(Student student)
        {
            var db = DbConnection();

            var command =
                "UPDATE Students SET student_name = @name, student_nickname = @nickname, character_id = @characterId, student_image = @userImage, student_total_score = @totalScore WHERE student_id = @id";

            try
            {
                db.Open();
                var result = await db.ExecuteAsync(command, new
                {
                    name = student.Name,
                    nickname = student.Nickname,
                    characterId = student.CharacterId,
                    userImage = student.Image,
                    totalScore = student.TotalScore,
                    id = student.Id
                });
                return result > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> InsertUser(Student student)
        {
            var db = DbConnection();
            
            var command =
                "INSERT INTO Students (student_name, student_email, student_nickname, character_id, student_image, student_total_score) VALUES (@name, @email, @nickname, @characterId, @userImage, @totalScore)";

            try
            {
                db.Open();
                var result = await db.ExecuteAsync(command, new
                {
                    name = student.Name,
                    email = student.Email,
                    nickname = student.Nickname,
                    characterId = student.CharacterId,
                    userImage = student.Image,
                    totalScore = student.TotalScore
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
