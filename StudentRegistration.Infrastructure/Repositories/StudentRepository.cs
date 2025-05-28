using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentRegistrationDbContext _context;

        public StudentRepository(StudentRegistrationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public void Delete(Student student)
        {
            _context.Students.Remove(student);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }

        public void Update(Student student)
        {
            _context.Students.Update(student);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.StudentId == id);
        }
    }
}
