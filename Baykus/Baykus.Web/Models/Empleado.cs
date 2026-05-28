using Baykus.Web.Data;
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

        [Display(Name = "Puesto")]
        public int? PuestoId { get; set; }

        public Puesto? Puesto { get; set; }

        [Display(Name = "Sector")]
        public int? SectorId { get; set; }

        public Sector? Sector { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public string? Legajo { get; set; }
        public string? Cuil { get; set; }

        public int? JornadaLaboralId { get; set; }
        public JornadaLaboral? JornadaLaboral { get; set; }

        public bool RequiereCargaHoraria { get; set; } = true;

        public DateTime? FechaBaja { get; set; }
        public string? MotivoBaja { get; set; }

        public ICollection<PlanillaHoraria> PlanillasHorarias { get; set; } = new List<PlanillaHoraria>();
        public ICollection<EmpleadoHistorialLaboral> HistorialLaboral { get; set; } = new List<EmpleadoHistorialLaboral>();
        public ICollection<EmpleadoPerfil> EmpleadoPerfiles { get; set; } = new List<EmpleadoPerfil>();

        public string? ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
