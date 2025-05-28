using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly StudentRegistrationDbContext _context;

        public SubjectRepository(StudentRegistrationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        public async Task<Subject?> GetByNameAsync(string name)
        {
            return await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectName == name);
        }
    }
}
