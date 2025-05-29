
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

            // Configuración para la entidad de unión ProfessorSubject
            modelBuilder.Entity<ProfessorSubject>(entity =>
            {
                // Nombre correcto de la tabla SQL
                entity.ToTable("professor_subjects");

                // Clave primaria compuesta
                entity.HasKey(ps => new { ps.ProfessorId, ps.SubjectId });

                // Nombres de columnas
                entity.Property(ps => ps.ProfessorId).HasColumnName("professor_id");
                entity.Property(ps => ps.SubjectId).HasColumnName("subject_id");

                // Relación con Professor
                entity.HasOne(ps => ps.Professor)
                    .WithMany(p => p.ProfessorSubjects) // Propiedad de navegación en Professor
                    .HasForeignKey(ps => ps.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull) // O Restrict / Cascade según diseño
                    .HasConstraintName("FK_professor_subjects_professors"); 

                // Relación con Subject
                entity.HasOne(ps => ps.Subject)
                    .WithMany(s => s.ProfessorSubjects) // Propiedad de navegación en Subject
                    .HasForeignKey(ps => ps.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull) // O Restrict / Cascade
                    .HasConstraintName("FK_professor_subjects_subjects"); 
            });

            // Configuración para la entidad de unión StudentSubject (Inscripciones)
            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.ToTable("student_subjects");
                entity.HasKey(ss => ss.StudentSubjectId); // Clave primaria simple
                entity.Property(ss => ss.StudentSubjectId).HasColumnName("student_subject_id").ValueGeneratedOnAdd();

                entity.Property(ss => ss.StudentId).HasColumnName("student_id");
                entity.Property(ss => ss.SubjectId).HasColumnName("subject_id");
                entity.Property(ss => ss.ProfessorId).HasColumnName("professor_id"); // Profesor que dicta esta materia a este estudiante
                entity.Property(ss => ss.EnrollmentDate).HasColumnType("datetime").HasColumnName("enrollment_date");

                entity.HasOne(ss => ss.Student)
                    .WithMany(s => s.StudentSubjects)
                    .HasForeignKey(ss => ss.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_subjects_students");

                entity.HasOne(ss => ss.Subject)
                    .WithMany(s => s.StudentSubjects) 
                    .HasForeignKey(ss => ss.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_subjects_subjects");

                entity.HasOne(ss => ss.Professor) // Relación con el profesor específico de esta inscripción
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(ss => ss.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_subjects_professors");
            });
        }
    }
}
