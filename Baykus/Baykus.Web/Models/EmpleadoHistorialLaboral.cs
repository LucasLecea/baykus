namespace Baykus.Web.Models
{
    public class EmpleadoHistorialLaboral
    {
        public int Id { get; set; }

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; } = null!;

        public int? SectorId { get; set; }
        public Sector? Sector { get; set; }

        public int? PuestoId { get; set; }
        public Puesto? Puesto { get; set; }

        public int? JornadaLaboralId { get; set; }
        public JornadaLaboral? JornadaLaboral { get; set; }

        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public string? Observacion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
