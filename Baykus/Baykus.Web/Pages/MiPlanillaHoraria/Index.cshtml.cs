using Baykus.Web.Data;
using Baykus.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Baykus.Web.Pages.MiPlanillaHoraria
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Empleado? Empleado { get; set; }
        public List<PlanillaHoraria> Planillas { get; set; } = new();

        public async Task OnGetAsync()
        {
            var emailUsuario = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(emailUsuario))
                return;

            Empleado = await _context.Empleados
                .Include(e => e.Sector)
                .Include(e => e.Puesto)
                .FirstOrDefaultAsync(e => e.Email == emailUsuario);

            if (Empleado == null)
                return;

            Planillas = await _context.PlanillasHorarias
                .Where(p => p.EmpleadoId == Empleado.Id)
                .OrderByDescending(p => p.Anio)
                .ThenByDescending(p => p.Mes)
                .ToListAsync();
        }
    }
}