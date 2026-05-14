using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(150)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Dni { get; set; }

        [StringLength(150)]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Telefono { get; set; }

        [StringLength(100)]
        public string? Puesto { get; set; }

        [StringLength(100)]
        public string? Sector { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaAlta { get; set; } = DateTime.Now;
    }
}
