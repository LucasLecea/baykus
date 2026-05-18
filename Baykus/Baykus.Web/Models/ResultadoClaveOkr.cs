using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class ResultadoClaveOkr
    {
        public int Id { get; set; }

        public int ObjetivoOkrId { get; set; }
        public ObjetivoOkr ObjetivoOkr { get; set; } = default!;

        [Required(ErrorMessage = "Debe ingresar una descripción para el resultado clave.")]
        [StringLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "El progreso debe estar entre 0 y 100.")]
        public int Progreso { get; set; } = 0;

        [StringLength(30)]
        public string Estado { get; set; } = "Pendiente";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
