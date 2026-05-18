using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class Puesto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del puesto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        [Display(Name = "Nombre del puesto")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Display(Name = "Sector")]
        public int? SectorId { get; set; }

        public Sector? Sector { get; set; }
    }
}
