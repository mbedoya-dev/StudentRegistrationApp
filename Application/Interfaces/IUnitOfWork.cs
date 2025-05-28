namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ISubjectRepository Subjects { get; }
        IProfessorRepository Professors { get; }
        // IStudentSubjectRepository StudentSubjects { get; } // Puedes añadir este si necesitas operaciones directas aquí

        Task<int> CompleteAsync();
    }
}
