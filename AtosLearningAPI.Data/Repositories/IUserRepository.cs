using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtosLearningAPI.Model;
namespace AtosLearningAPI.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<bool> InsertUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user); 
    }
}
