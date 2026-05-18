using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Baykus.Web.Data;
using Baykus.Web.Models;

namespace Baykus.Web.Pages.OkrPages;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public ObjetivoOkr Objetivo { get; set; } = new();

    public SelectList SectoresSelectList { get; set; } = default!;
    public SelectList PuestosSelectList { get; set; } = default!;
    public SelectList EmpleadosSelectList { get; set; } = default!;

    public string PuestosJson { get; set; } = "[]";
    public string EmpleadosJson { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync()
    {
        await CargarCombosAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidarObjetivo();

        if (!ModelState.IsValid)
        {
            await CargarCombosAsync(Objetivo.SectorId, Objetivo.PuestoId, Objetivo.EmpleadoResponsableId);
            return Page();
        }

        Objetivo.Progreso = 0;
        Objetivo.Estado = "Pendiente";
        Objetivo.FechaCreacion = DateTime.Now;

        _context.ObjetivosOkr.Add(Objetivo);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Details", new { id = Objetivo.Id });
    }

    private void ValidarObjetivo()
    {
        if (Objetivo.SectorId == null)
        {
            ModelState.AddModelError("Objetivo.SectorId", "Debe seleccionar un sector.");
        }

        if (Objetivo.EmpleadoResponsableId == null)
        {
            ModelState.AddModelError("Objetivo.EmpleadoResponsableId", "Debe seleccionar un responsable.");
        }

        if (Objetivo.FechaFin.Date < Objetivo.FechaInicio.Date)
        {
            ModelState.AddModelError("Objetivo.FechaFin", "La fecha de fin no puede ser anterior a la fecha de inicio.");
        }
    }

    private async Task CargarCombosAsync(int? sectorId = null, int? puestoId = null, int? empleadoId = null)
    {
        var sectores = await _context.Sector
            .AsNoTracking()
            .Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        var puestos = await _context.Puestos
            .AsNoTracking()
            .Where(p => p.Activo && p.SectorId != null)
            .OrderBy(p => p.Nombre)
            .Select(p => new
            {
                id = p.Id,
                nombre = p.Nombre,
                sectorId = p.SectorId
            })
            .ToListAsync();

        var empleados = await _context.Empleados
            .AsNoTracking()
            .Where(e => e.Activo)
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .Select(e => new
            {
                id = e.Id,
                nombre = e.Nombre + " " + e.Apellido,
                sectorId = e.SectorId,
                puestoId = e.PuestoId
            })
            .ToListAsync();

        SectoresSelectList = new SelectList(sectores, "Id", "Nombre", sectorId);

        PuestosSelectList = new SelectList(
            puestos.Where(p => p.sectorId == sectorId),
            "id",
            "nombre",
            puestoId
        );

        EmpleadosSelectList = new SelectList(
            empleados.Where(e =>
                e.sectorId == sectorId &&
                (puestoId == null || e.puestoId == puestoId)),
            "id",
            "nombre",
            empleadoId
        );

        PuestosJson = JsonSerializer.Serialize(puestos);
        EmpleadosJson = JsonSerializer.Serialize(empleados);
    }
}