using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class Modulo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        public string Codigo { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
