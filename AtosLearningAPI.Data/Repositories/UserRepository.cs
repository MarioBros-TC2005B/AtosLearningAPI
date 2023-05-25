using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtosLearningAPI.Model;
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

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString); 
        }


        public async Task<bool> DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertUser(User user)
        {
            throw new NotImplementedException(); 
        }

    }
}
