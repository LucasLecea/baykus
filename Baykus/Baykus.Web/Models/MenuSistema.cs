using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class MenuSistema
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        public string Codigo { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        [StringLength(100)]
        public string? Icono { get; set; }

        [StringLength(300)]
        public string? Url { get; set; }

        [StringLength(300)]
        public string? Page { get; set; }

        [StringLength(100)]
        public string? Area { get; set; }

        public int Orden { get; set; }

        public bool Activo { get; set; } = true;

        public bool EsVisible { get; set; } = true;

        public int? MenuPadreId { get; set; }

        public MenuSistema? MenuPadre { get; set; }

        public ICollection<MenuSistema> SubMenus { get; set; } = new List<MenuSistema>();

        public ICollection<PerfilMenuPermiso> PerfilPermisos { get; set; } = new List<PerfilMenuPermiso>();

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
