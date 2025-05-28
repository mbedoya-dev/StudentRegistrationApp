using StudentRegistration.Domain.Entities;

namespace Application.Interfaces
{
    public interface IProfessorRepository
    {
        Task<Professor?> GetByIdAsync(int id);
        Task<IEnumerable<Professor>> GetAllAsync();
        Task<Professor?> GetByEmailAsync(string email);
    }
}
