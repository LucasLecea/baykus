using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Data;
using Baykus.Web.Models;
using Baykus.Web.Services;

namespace Baykus.Web.Pages.OkrPages;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public ObjetivoOkr Objetivo { get; set; } = default!;

    [BindProperty]
    public ResultadoClaveOkr NuevoResultado { get; set; } = new();

    [BindProperty]
    public SeguimientoOkr NuevoSeguimiento { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var objetivo = await ObtenerObjetivoAsync(id.Value);

        if (objetivo is null)
        {
            return NotFound();
        }

        OkrService.ActualizarObjetivo(objetivo);
        await _context.SaveChangesAsync();

        Objetivo = objetivo;

        return Page();
    }

    public async Task<IActionResult> OnPostAgregarResultadoAsync(int id)
    {
        var objetivo = await ObtenerObjetivoAsync(id);

        if (objetivo is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(NuevoResultado.Descripcion))
        {
            TempData["Error"] = "Debe ingresar una descripción para el resultado clave.";
            return RedirectToPage("./Details", new { id });
        }

        NuevoResultado.ObjetivoOkrId = id;
        NuevoResultado.Progreso = 0;
        NuevoResultado.Estado = "Pendiente";
        NuevoResultado.FechaCreacion = DateTime.Now;

        _context.ResultadosClaveOkr.Add(NuevoResultado);

        objetivo.ResultadosClave.Add(NuevoResultado);
        OkrService.ActualizarObjetivo(objetivo);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Details", new { id });
    }

    public async Task<IActionResult> OnPostActualizarResultadoAsync(int id, int resultadoId, int progreso)
    {
        var objetivo = await ObtenerObjetivoAsync(id);

        if (objetivo is null)
        {
            return NotFound();
        }

        var resultado = objetivo.ResultadosClave.FirstOrDefault(r => r.Id == resultadoId);

        if (resultado is null)
        {
            return NotFound();
        }

        resultado.Progreso = progreso;
        OkrService.ActualizarEstadoResultado(resultado);
        OkrService.ActualizarObjetivo(objetivo);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Details", new { id });
    }

    public async Task<IActionResult> OnPostEliminarResultadoAsync(int id, int resultadoId)
    {
        var objetivo = await ObtenerObjetivoAsync(id);

        if (objetivo is null)
        {
            return NotFound();
        }

        var resultado = objetivo.ResultadosClave.FirstOrDefault(r => r.Id == resultadoId);

        if (resultado is null)
        {
            return NotFound();
        }

        _context.ResultadosClaveOkr.Remove(resultado);

        OkrService.ActualizarObjetivo(objetivo);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Details", new { id });
    }

    public async Task<IActionResult> OnPostAgregarSeguimientoAsync(int id)
    {
        var objetivo = await ObtenerObjetivoAsync(id);

        if (objetivo is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(NuevoSeguimiento.Comentario))
        {
            TempData["Error"] = "Debe ingresar un comentario.";
            return RedirectToPage("./Details", new { id });
        }

        NuevoSeguimiento.ObjetivoOkrId = id;
        NuevoSeguimiento.Fecha = DateTime.Now;
        NuevoSeguimiento.Usuario = User.Identity?.Name;

        _context.SeguimientosOkr.Add(NuevoSeguimiento);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Details", new { id });
    }

    private async Task<ObjetivoOkr?> ObtenerObjetivoAsync(int id)
    {
        return await _context.ObjetivosOkr
            .Include(o => o.Sector)
            .Include(o => o.Puesto)
            .Include(o => o.EmpleadoResponsable)
            .Include(o => o.ResultadosClave)
            .Include(o => o.Seguimientos)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}