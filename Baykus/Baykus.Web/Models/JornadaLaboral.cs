namespace Baykus.Web.Models
{
    public class JornadaLaboral
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public decimal HorasDiariasObjetivo { get; set; }
        public decimal HorasSemanalesObjetivo { get; set; }

        public TimeOnly? HoraEntradaDefault { get; set; }
        public TimeOnly? HoraSalidaDefault { get; set; }

        public bool PermiteHorasExtra { get; set; } = true;
        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
