namespace Baykus.Web.Models
{
    public class PlanillaHorariaDetalle
    {
        public int Id { get; set; }

        public int PlanillaHorariaId { get; set; }
        public PlanillaHoraria PlanillaHoraria { get; set; } = null!;

        public DateOnly Fecha { get; set; }

        public TimeOnly? HoraEntrada { get; set; }
        public TimeOnly? HoraSalida { get; set; }

        public int MinutosDescanso { get; set; }

        public decimal HorasNormales { get; set; }
        public decimal HorasExtra50 { get; set; }
        public decimal HorasExtra100 { get; set; }
        public decimal HorasNocturnas { get; set; }
        public decimal HorasFeriado { get; set; }

        public decimal TotalHoras { get; set; }

        public TipoDiaPlanilla TipoDia { get; set; } = TipoDiaPlanilla.Trabajado;

        public bool EsFeriado { get; set; }
        public bool EsFinDeSemana { get; set; }

        public string? TareaRealizada { get; set; }
        public string? Observacion { get; set; }

        public DateTime FechaCarga { get; set; } = DateTime.Now;
        public DateTime? FechaUltimaModificacion { get; set; }
    }
    public enum TipoDiaPlanilla
    {
        Trabajado = 1,
        Franco = 2,
        Ausente = 3,
        Licencia = 4,
        Vacaciones = 5,
        Feriado = 6,
        Suspencion = 7
    }

    public enum TipoAusencia
    {
        SinJustificar = 1,
        Enfermedad = 2,
        Vacaciones = 3,
        Licencia = 4,
        PermisoEspecial = 5,
        Feriado = 6,
        Franco = 7
    }
}
