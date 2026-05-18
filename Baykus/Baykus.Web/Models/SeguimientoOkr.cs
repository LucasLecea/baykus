using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class SeguimientoOkr
    {
        public int Id { get; set; }

        public int ObjetivoOkrId { get; set; }
        public ObjetivoOkr ObjetivoOkr { get; set; } = default!;

        [Required(ErrorMessage = "Debe ingresar un comentario.")]
        [StringLength(1000, ErrorMessage = "El comentario no puede superar los 1000 caracteres.")]
        public string Comentario { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        public string? Usuario { get; set; }
    }
}
