using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtosLearningAPI.Model;
namespace AtosLearningAPI.Data.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllUsers();
        Task<Student> GetUserById(int id);
        Task<bool> InsertUser(Student student);
        Task<bool> UpdateUser(Student student);
        Task<bool> DeleteUser(Student student); 
    }
}
