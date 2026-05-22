namespace Baykus.Web.Models
{
    public class PlanillaHorariaHistorial
    {
        public int Id { get; set; }

        public int PlanillaHorariaId { get; set; }
        public PlanillaHoraria PlanillaHoraria { get; set; } = null!;

        public EstadoPlanillaHoraria? EstadoAnterior { get; set; }
        public EstadoPlanillaHoraria EstadoNuevo { get; set; }

        public string? Comentario { get; set; }

        public string? UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
