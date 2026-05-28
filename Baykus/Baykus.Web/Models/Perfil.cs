using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class Perfil
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public bool EsAdministrador { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<EmpleadoPerfil> EmpleadoPerfiles { get; set; } = new List<EmpleadoPerfil>();

        public ICollection<PerfilMenuPermiso> MenuPermisos { get; set; } = new List<PerfilMenuPermiso>();
    }
}
