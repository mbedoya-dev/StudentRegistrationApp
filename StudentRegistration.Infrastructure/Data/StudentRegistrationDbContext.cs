
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

            // Mapeo para la entidad Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId).HasColumnName("student_id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");
                entity.Property(e => e.LastUpdated).HasColumnName("last_updated");

                // Configuración para las propiedades de fecha que ya tenías
                entity.Property(s => s.RegistrationDate)
                      .HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.LastUpdated)
                      .HasDefaultValueSql("GETDATE()");
            });

            // Mapeo para la entidad Subject
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.SubjectId).HasColumnName("subject_id");
                entity.Property(e => e.SubjectName).HasColumnName("subject_name");
                entity.Property(e => e.Credits).HasColumnName("credits");
            });

            // Mapeo para la entidad Professor
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.Property(e => e.ProfessorId).HasColumnName("professor_id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.Email).HasColumnName("email");
            });

            // Configuración para la tabla ProfessorSubject (relación muchos a muchos)
            modelBuilder.Entity<ProfessorSubject>(entity => // Añadido alias 'entity' para consistencia
            {
                entity.HasKey(ps => new { ps.ProfessorId, ps.SubjectId });
                entity.Property(ps => ps.ProfessorId).HasColumnName("professor_id"); 
                entity.Property(ps => ps.SubjectId).HasColumnName("subject_id");

                entity.HasOne(ps => ps.Professor)
                      .WithMany(p => p.ProfessorSubjects)
                      .HasForeignKey(ps => ps.ProfessorId);

                entity.HasOne(ps => ps.Subject)
                      .WithMany(s => s.ProfessorSubjects)
                      .HasForeignKey(ps => ps.SubjectId);
            });


            // Configuración para la tabla StudentSubject
            modelBuilder.Entity<StudentSubject>(entity => 
            {
                entity.HasKey(ss => ss.StudentSubjectId); // Clave primaria definida en la entidad
                entity.Property(ss => ss.StudentSubjectId).HasColumnName("student_subject_id");
                entity.Property(ss => ss.StudentId).HasColumnName("student_id");
                entity.Property(ss => ss.SubjectId).HasColumnName("subject_id");
                entity.Property(ss => ss.ProfessorId).HasColumnName("professor_id");
                entity.Property(ss => ss.EnrollmentDate).HasColumnName("enrollment_date");


                // Relación Student a StudentSubject
                entity.HasOne(ss => ss.Student)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(ss => ss.StudentId);

                // Relación Subject a StudentSubject
                entity.HasOne(ss => ss.Subject)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(ss => ss.SubjectId);

                // Relación Professor a StudentSubject
                entity.HasOne(ss => ss.Professor)
                      .WithMany(p => p.StudentSubjects)
                      .HasForeignKey(ss => ss.ProfessorId);

                // Configurar la restricción única (UQ_StudentSubject)
                entity.HasIndex(ss => new { ss.StudentId, ss.SubjectId })
                      .IsUnique();

                // Configuración para las propiedades de fecha que ya tenías
                entity.Property(ss => ss.EnrollmentDate)
                      .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
