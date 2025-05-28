
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data
{
    public class StudentRegistrationDbContext : DbContext
    {
        public StudentRegistrationDbContext(DbContextOptions<StudentRegistrationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Professor> Professors { get; set; } = null!;
        public DbSet<ProfessorSubject> ProfessorSubjects { get; set; } = null!;
        public DbSet<StudentSubject> StudentSubjects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la tabla ProfessorSubject (relación muchos a muchos)
            modelBuilder.Entity<ProfessorSubject>()
                .HasKey(ps => new { ps.ProfessorId, ps.SubjectId });

            modelBuilder.Entity<ProfessorSubject>()
                .HasOne(ps => ps.Professor)
                .WithMany(p => p.ProfessorSubjects)
                .HasForeignKey(ps => ps.ProfessorId);

            modelBuilder.Entity<ProfessorSubject>()
                .HasOne(ps => ps.Subject)
                .WithMany(s => s.ProfessorSubjects)
                .HasForeignKey(ps => ps.SubjectId);

            // Configuración para la tabla StudentSubject
            modelBuilder.Entity<StudentSubject>()
                .HasKey(ss => ss.StudentSubjectId); // Clave primaria definida en la entidad

            // Relación Student a StudentSubject
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId);

            // Relación Subject a StudentSubject
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId);

            // Relación Professor a StudentSubject
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Professor)
                .WithMany(p => p.StudentSubjects) 
                .HasForeignKey(ss => ss.ProfessorId);

            // Configurar la restricción única (UQ_StudentSubject)
            modelBuilder.Entity<StudentSubject>()
                .HasIndex(ss => new { ss.StudentId, ss.SubjectId })
                .IsUnique();

            // Configuración para las propiedades de fecha
            modelBuilder.Entity<Student>()
                .Property(s => s.RegistrationDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Student>()
                .Property(s => s.LastUpdated)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<StudentSubject>()
                .Property(ss => ss.EnrollmentDate)
                .HasDefaultValueSql("GETDATE()");           
        }
    }
}
