using AtosLearningAPI.Model;
namespace AtosLearningAPI.Data.Repositories;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAllSubjects();
    Task<Subject> GetSubjectById(int id);
    Task<bool> AddSubject(Subject subject);
    Task<bool> UpdateSubject(Subject subject);
    Task<bool> DeleteSubject(int id);
}