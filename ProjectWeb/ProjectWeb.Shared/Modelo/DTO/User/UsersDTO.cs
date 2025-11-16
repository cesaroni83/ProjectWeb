using Microsoft.AspNetCore.Identity;
using ProjectWeb.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using ProjectWeb.Shared.Modelo.DTO.Ciudad;
namespace ProjectWeb.Shared.Modelo.DTO.User
{
    public class UsersDTO : IdentityUser
    {
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Foto")]
        public string? Photo { get; set; }

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        public CiudadDTO? Ciudades { get; set; }

        public string Nombre_Ciudad => Ciudades?.Nombre_ciudad ?? "";

        [Display(Name = "Ciudad")]
       // [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int Id_ciudad { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        public string? Passwords { get; set; } = null!;
    }
}
