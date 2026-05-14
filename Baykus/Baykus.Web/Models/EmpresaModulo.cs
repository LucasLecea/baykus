namespace Baykus.Web.Models
{
    public class EmpresaModulo
    {
        public int Id { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public int ModuloId { get; set; }
        public Modulo Modulo { get; set; } = null!;

        public bool Habilitado { get; set; } = true;
    }
}
