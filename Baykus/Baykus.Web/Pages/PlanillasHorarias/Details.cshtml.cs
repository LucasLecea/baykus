using Baykus.Web.Data;
using Baykus.Web.Models;
using Baykus.Web.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Baykus.Web.Pages.PlanillasHorarias
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PlanillaHoraria? Planilla { get; set; }

        public List<PlanillaHorariaDetalle> Detalles { get; set; } = new();

        public List<PlanillaHorariaHistorial> Historial { get; set; } = new();

        [BindProperty]
        public string? ObservacionAprobador { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await CargarPlanillaAsync(id);

            if (Planilla == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAprobarAsync(int id)
        {
            var planilla = await _context.PlanillasHorarias
                .FirstOrDefaultAsync(p => p.Id == id);

            if (planilla == null)
                return NotFound();

            if (planilla.Estado != EstadoPlanillaHoraria.Presentada)
            {
                TempData["Error"] = "Solo se pueden aprobar planillas que estén presentadas.";
                return RedirectToPage("./Details", new { id });
            }

            var estadoAnterior = planilla.Estado;

            planilla.Estado = EstadoPlanillaHoraria.Aprobada;
            planilla.FechaAprobacion = DateTime.Now;
            planilla.FechaRechazo = null;
            planilla.ObservacionAprobador = string.IsNullOrWhiteSpace(ObservacionAprobador)
                ? "Planilla revisada y aprobada por RRHH."
                : ObservacionAprobador;

            planilla.UsuarioAprobacionId = User.Identity?.Name ?? "rrhh";

            _context.PlanillasHorariasHistorial.Add(new PlanillaHorariaHistorial
            {
                PlanillaHorariaId = planilla.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = EstadoPlanillaHoraria.Aprobada,
                Comentario = planilla.ObservacionAprobador,
                UsuarioId = User.Identity?.Name ?? "rrhh",
                Fecha = DateTime.Now
            });

            await _context.SaveChangesAsync();

            TempData["Success"] = "La planilla fue aprobada correctamente.";
            return RedirectToPage("./Details", new { id });
        }

        public async Task<IActionResult> OnPostRechazarAsync(int id)
        {
            var planilla = await _context.PlanillasHorarias
                .FirstOrDefaultAsync(p => p.Id == id);

            if (planilla == null)
                return NotFound();

            if (planilla.Estado != EstadoPlanillaHoraria.Presentada)
            {
                TempData["Error"] = "Solo se pueden rechazar planillas que estén presentadas.";
                return RedirectToPage("./Details", new { id });
            }

            if (string.IsNullOrWhiteSpace(ObservacionAprobador))
            {
                TempData["Error"] = "Para rechazar una planilla tenés que indicar una observación.";
                return RedirectToPage("./Details", new { id });
            }

            var estadoAnterior = planilla.Estado;

            planilla.Estado = EstadoPlanillaHoraria.Rechazada;
            planilla.FechaRechazo = DateTime.Now;
            planilla.FechaAprobacion = null;
            planilla.ObservacionAprobador = ObservacionAprobador;
            planilla.UsuarioAprobacionId = User.Identity?.Name ?? "rrhh";

            _context.PlanillasHorariasHistorial.Add(new PlanillaHorariaHistorial
            {
                PlanillaHorariaId = planilla.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = EstadoPlanillaHoraria.Rechazada,
                Comentario = ObservacionAprobador,
                UsuarioId = User.Identity?.Name ?? "rrhh",
                Fecha = DateTime.Now
            });

            await _context.SaveChangesAsync();

            TempData["Success"] = "La planilla fue rechazada correctamente.";
            return RedirectToPage("./Details", new { id });
        }

        private async Task CargarPlanillaAsync(int id)
        {
            Planilla = await _context.PlanillasHorarias
                .Include(p => p.Empleado)
                    .ThenInclude(e => e.Sector)
                .Include(p => p.Empleado)
                    .ThenInclude(e => e.Puesto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Planilla == null)
                return;

            Detalles = await _context.PlanillasHorariasDetalles
                .Where(d => d.PlanillaHorariaId == id)
                .OrderBy(d => d.Fecha)
                .ToListAsync();

            Historial = await _context.PlanillasHorariasHistorial
                .Where(h => h.PlanillaHorariaId == id)
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();
        }
    }
}