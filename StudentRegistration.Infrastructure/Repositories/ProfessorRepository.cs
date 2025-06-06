﻿using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly StudentRegistrationDbContext _context;

        public ProfessorRepository(StudentRegistrationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Professor>> GetAllAsync()
        {
            return await _context.Professors.ToListAsync();
        }

        public async Task<Professor?> GetByIdAsync(int id)
        {
            return await _context.Professors.FindAsync(id);
        }

        public async Task<Professor?> GetByEmailAsync(string email)
        {
            return await _context.Professors.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Professor?> GetByIdWithSubjectsAsync(int professorId)
        {
            return await _context.Professors
                                 .Include(p => p.ProfessorSubjects)
                                 .FirstOrDefaultAsync(p => p.ProfessorId == professorId);
        }
    }
}
