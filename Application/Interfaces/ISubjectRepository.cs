using StudentRegistration.Domain.Entities;

namespace Application.Interfaces
{
    public interface ISubjectRepository
    {
        Task<Subject?> GetByIdAsync(int id);
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<Subject?> GetByNameAsync(string name);
    }
}
