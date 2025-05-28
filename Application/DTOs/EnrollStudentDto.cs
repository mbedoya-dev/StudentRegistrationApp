using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Application.DTOs
{
    public class EnrollStudentDto
    {
        [Required(ErrorMessage = "El ID del estudiante es requerido.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Las materias seleccionadas son requeridas.")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una materia.")]
        [MaxLength(3, ErrorMessage = "Un estudiante solo puede seleccionar un máximo de 3 materias.")]
        public List<SubjectEnrollmentDetailDto> Enrollments { get; set; } = new List<SubjectEnrollmentDetailDto>();
    }

    public class SubjectEnrollmentDetailDto
    {
        [Required(ErrorMessage = "El ID de la materia es requerido.")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "El ID del profesor es requerido para la materia.")]
        public int ProfessorId { get; set; }
    }
}
