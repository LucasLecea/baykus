using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Dni { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Puesto { get; set; }

        [StringLength(100)]
        public string? Sector { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public bool Activo { get; set; } = true;
    }
}
