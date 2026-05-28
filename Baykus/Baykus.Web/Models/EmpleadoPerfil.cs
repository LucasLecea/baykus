using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class EmpleadoPerfil
    {
        public int Id { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        public Empleado Empleado { get; set; } = default!;

        [Required]
        public int PerfilId { get; set; }

        public Perfil Perfil { get; set; } = default!;

        public bool Activo { get; set; } = true;

        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}
