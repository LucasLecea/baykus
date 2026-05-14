using System.ComponentModel.DataAnnotations;

namespace Baykus.Web.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Cuit { get; set; }

        [StringLength(250)]
        public string? EmailContacto { get; set; }

        [StringLength(250)]
        public string? TelefonoContacto { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaAlta { get; set; } = DateTime.Now;
    }
}
