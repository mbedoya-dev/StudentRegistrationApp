
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudentRegistrationDbContext _context;
        private IStudentRepository? _students;
        private ISubjectRepository? _subjects;
        private IProfessorRepository? _professors;
        private IStudentSubjectRepository? _studentSubjects;

        public UnitOfWork(StudentRegistrationDbContext context)
        {
            _context = context;
        }

        public IStudentRepository Students => _students ??= new StudentRepository(_context);
        public ISubjectRepository Subjects => _subjects ??= new SubjectRepository(_context);
        public IProfessorRepository Professors => _professors ??= new ProfessorRepository(_context);
        public IStudentSubjectRepository StudentSubjects => _studentSubjects ??= new StudentSubjectRepository(_context); 

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
