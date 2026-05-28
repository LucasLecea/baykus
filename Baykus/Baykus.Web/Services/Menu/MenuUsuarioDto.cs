namespace Baykus.Web.Services.Menu
{
    public class MenuUsuarioDto
    {
        public int Id { get; set; }
        public int? MenuPadreId { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;

        public string? Icono { get; set; }
        public string? Url { get; set; }
        public string? Page { get; set; }
        public string? Area { get; set; }

        public int Orden { get; set; }

        public bool PuedeVer { get; set; }
        public bool PuedeCrear { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEliminar { get; set; }
        public bool PuedeAprobar { get; set; }
        public bool PuedeRevisar { get; set; }

        public List<MenuUsuarioDto> Hijos { get; set; } = new();
    }
}
