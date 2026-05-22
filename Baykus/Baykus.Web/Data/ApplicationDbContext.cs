using Baykus.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Baykus.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<EmpresaModulo> EmpresaModulos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<Sector> Sector { get; set; }
        public DbSet<ObjetivoOkr> ObjetivosOkr { get; set; }
        public DbSet<ResultadoClaveOkr> ResultadosClaveOkr { get; set; }
        public DbSet<SeguimientoOkr> SeguimientosOkr { get; set; }
        public DbSet<JornadaLaboral> JornadasLaborales { get; set; }
        public DbSet<EmpleadoHistorialLaboral> EmpleadosHistorialLaboral { get; set; }
        public DbSet<PlanillaHoraria> PlanillasHorarias { get; set; }
        public DbSet<PlanillaHorariaDetalle> PlanillasHorariasDetalles { get; set; }
        public DbSet<PlanillaHorariaHistorial> PlanillasHorariasHistorial { get; set; }
        public DbSet<Feriado> Feriados { get; set; }
    }
}
