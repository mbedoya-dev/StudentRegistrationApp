using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories
{
    public class StudentSubjectRepository : IStudentSubjectRepository
    {
        private readonly StudentRegistrationDbContext _context;

        public StudentSubjectRepository(StudentRegistrationDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<StudentSubject> studentSubjects)
        {
            await _context.StudentSubjects.AddRangeAsync(studentSubjects);
        }

        public async Task<IEnumerable<StudentSubject>> GetStudentEnrollmentsWithDetailsAsync(int studentId)
        {
            return await _context.StudentSubjects
                                 .Where(ss => ss.StudentId == studentId)
                                 .Include(ss => ss.Subject)
                                 .Include(ss => ss.Professor)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<StudentSubject>> GetAllEnrollmentsWithDetailsAsync()
        {
            return await _context.StudentSubjects
                                 .Include(ss => ss.Student)
                                 .Include(ss => ss.Subject)
                                 .Include(ss => ss.Professor)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<StudentSubject>> GetStudentsInSubjectWithDetailsAsync(int subjectId, int? excludeStudentId = null)
        {
            var query = _context.StudentSubjects
                                .Where(ss => ss.SubjectId == subjectId);

            if (excludeStudentId.HasValue)
            {
                query = query.Where(ss => ss.StudentId != excludeStudentId.Value);
            }

            return await query
                        .Include(ss => ss.Student) // Necesitas el objeto Student para el DTO
                        .ToListAsync();
        }

        public async Task<bool> HasStudentProfessorInEnrollmentsAsync(int studentId, int professorId)
        {
            return await _context.StudentSubjects
                                 .AnyAsync(ss => ss.StudentId == studentId && ss.ProfessorId == professorId);
        }

        public async Task<bool> IsStudentEnrolledInSubjectAsync(int studentId, int subjectId)
        {
            return await _context.StudentSubjects
                                 .AnyAsync(ss => ss.StudentId == studentId && ss.SubjectId == subjectId);
        }

        public async Task<IEnumerable<StudentSubject>> GetStudentEnrollmentsCountAsync(int studentId)
        {
            return await _context.StudentSubjects
                                .Where(ss => ss.StudentId == studentId)
                                .ToListAsync();
        }
    }
}