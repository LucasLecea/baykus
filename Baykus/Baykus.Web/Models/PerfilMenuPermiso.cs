using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class PerfilMenuPermiso
    {
        public int Id { get; set; }

        [Required]
        public int PerfilId { get; set; }

        public Perfil Perfil { get; set; } = default!;

        [Required]
        public int MenuSistemaId { get; set; }

        public MenuSistema MenuSistema { get; set; } = default!;

        public bool PuedeVer { get; set; } = false;

        public bool PuedeCrear { get; set; } = false;

        public bool PuedeEditar { get; set; } = false;

        public bool PuedeEliminar { get; set; } = false;

        public bool PuedeAprobar { get; set; } = false;

        public bool PuedeRevisar { get; set; } = false;

        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}
