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
        public DbSet<Perfil> Perfiles { get; set; }
        public DbSet<EmpleadoPerfil> EmpleadoPerfiles { get; set; }
        public DbSet<MenuSistema> MenusSistema { get; set; }
        public DbSet<PerfilMenuPermiso> PerfilMenuPermisos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Empleado>()
                .HasOne(e => e.ApplicationUser)
                .WithOne(u => u.Empleado)
                .HasForeignKey<Empleado>(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Empleado>()
                .HasIndex(e => e.ApplicationUserId)
                .IsUnique()
                .HasFilter("[ApplicationUserId] IS NOT NULL");
        }
    }
}
