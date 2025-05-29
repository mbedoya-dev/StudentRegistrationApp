using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Interfaces
{
    public interface IProfessorRepository
    {
        Task<Professor?> GetByIdAsync(int id);
        Task<IEnumerable<Professor>> GetAllAsync();
        Task<Professor?> GetByEmailAsync(string email);
    }
}
