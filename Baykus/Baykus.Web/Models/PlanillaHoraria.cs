using Baykus.Web.Models.Enums;

namespace Baykus.Web.Models
{
    public class PlanillaHoraria
    {
        public int Id { get; set; }

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; } = null!;

        public DateOnly FechaDesde { get; set; }
        public DateOnly FechaHasta { get; set; }

        public int Anio { get; set; }
        public int Mes { get; set; }

        public EstadoPlanillaHoraria Estado { get; set; } = EstadoPlanillaHoraria.Borrador;

        public decimal TotalHorasNormales { get; set; }
        public decimal TotalHorasExtra50 { get; set; }
        public decimal TotalHorasExtra100 { get; set; }
        public decimal TotalHorasNocturnas { get; set; }
        public decimal TotalHorasFeriado { get; set; }
        public decimal TotalHorasTrabajadas { get; set; }

        public string? ObservacionEmpleado { get; set; }
        public string? ObservacionAprobador { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaPresentacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaRechazo { get; set; }

        public string? UsuarioCargaId { get; set; }
        public string? UsuarioAprobacionId { get; set; }

        // Snapshots históricos
        public string? EmpleadoNombreSnapshot { get; set; }
        public string? EmpleadoDniSnapshot { get; set; }
        public string? SectorNombreSnapshot { get; set; }
        public string? PuestoNombreSnapshot { get; set; }
        public string? JornadaNombreSnapshot { get; set; }

        public ICollection<PlanillaHorariaDetalle> Detalles { get; set; } = new List<PlanillaHorariaDetalle>();
    }
}
