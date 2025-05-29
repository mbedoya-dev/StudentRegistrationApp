using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Interfaces
{
    public interface ISubjectRepository
    {
        Task<Subject?> GetByIdAsync(int id);
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<Subject?> GetByNameAsync(string name);
        Task<IEnumerable<Subject>> GetAllWithProfessorAssignmentsAsync();
    }
}
