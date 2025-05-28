namespace StudentRegistration.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ISubjectRepository Subjects { get; }
        IProfessorRepository Professors { get; }
        IStudentSubjectRepository StudentSubjects { get; } // <--- ¡Añadido!

        Task<int> CompleteAsync();
    }
}
