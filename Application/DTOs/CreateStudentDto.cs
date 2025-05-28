using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Application.DTOs
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido.")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
        public string Email { get; set; } = string.Empty;
    }
}
