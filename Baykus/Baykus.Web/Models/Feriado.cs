namespace Baykus.Web.Models
{
    public class Feriado
    {
        public int Id { get; set; }

        public DateOnly Fecha { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool EsNacional { get; set; } = true;
        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
