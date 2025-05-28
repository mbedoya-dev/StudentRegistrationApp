using StudentRegistration.Domain.Entities;

namespace Application.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        Task<Student?> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(int id);
    }
}
