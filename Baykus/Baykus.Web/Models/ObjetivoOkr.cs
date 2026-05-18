using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class ObjetivoOkr
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título del objetivo es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un sector.")]
        public int? SectorId { get; set; }
        public Sector? Sector { get; set; }

        public int? PuestoId { get; set; }
        public Puesto? Puesto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un responsable.")]
        public int? EmpleadoResponsableId { get; set; }
        public Empleado? EmpleadoResponsable { get; set; }

        [Required(ErrorMessage = "Debe indicar la fecha de inicio.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debe indicar la fecha de fin.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; } = DateTime.Today.AddMonths(1);

        public int Progreso { get; set; } = 0;

        [StringLength(30)]
        public string Estado { get; set; } = "Pendiente";

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<ResultadoClaveOkr> ResultadosClave { get; set; } = new List<ResultadoClaveOkr>();

        public ICollection<SeguimientoOkr> Seguimientos { get; set; } = new List<SeguimientoOkr>();
    }
}
