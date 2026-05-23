using Baykus.Web.Data;
using Baykus.Web.Models;
using Baykus.Web.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Baykus.Web.Pages.PlanillasHorarias
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PlanillaHoraria> Planillas { get; set; } = new();

        public List<Empleado> Empleados { get; set; } = new();
        public List<Sector> Sectores { get; set; } = new();
        public List<Puesto> Puestos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? EmpleadoId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SectorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PuestoId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Mes { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Anio { get; set; }

        [BindProperty(SupportsGet = true)]
        public EstadoPlanillaHoraria? Estado { get; set; }

        public async Task OnGetAsync()
        {
            Empleados = await _context.Empleados
                .Where(e => e.Activo)
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();

            Sectores = await _context.Sector
                .Where(s => s.Activo)
                .OrderBy(s => s.Nombre)
                .ToListAsync();

            Puestos = await _context.Puestos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            var query = _context.PlanillasHorarias
                .Include(p => p.Empleado)
                    .ThenInclude(e => e.Sector)
                .Include(p => p.Empleado)
                    .ThenInclude(e => e.Puesto)
                .AsQueryable();

            if (EmpleadoId.HasValue)
                query = query.Where(p => p.EmpleadoId == EmpleadoId.Value);

            if (SectorId.HasValue)
                query = query.Where(p => p.Empleado.SectorId == SectorId.Value);

            if (PuestoId.HasValue)
                query = query.Where(p => p.Empleado.PuestoId == PuestoId.Value);

            if (Mes.HasValue)
                query = query.Where(p => p.Mes == Mes.Value);

            if (Anio.HasValue)
                query = query.Where(p => p.Anio == Anio.Value);

            if (Estado.HasValue)
                query = query.Where(p => p.Estado == Estado.Value);

            Planillas = await query
                .OrderByDescending(p => p.Anio)
                .ThenByDescending(p => p.Mes)
                .ThenBy(p => p.Empleado.Apellido)
                .ToListAsync();
        }
    }
}