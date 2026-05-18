using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Baykus.Web.Data;
using Baykus.Web.Models;
using Baykus.Web.Services;

namespace Baykus.Web.Pages.OkrPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public ObjetivoOkr Objetivo { get; set; } = default!;

    public SelectList SectoresSelectList { get; set; } = default!;
    public SelectList PuestosSelectList { get; set; } = default!;
    public SelectList EmpleadosSelectList { get; set; } = default!;

    public string PuestosJson { get; set; } = "[]";
    public string EmpleadosJson { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var objetivo = await _context.ObjetivosOkr
            .Include(o => o.Sector)
            .Include(o => o.Puesto)
            .Include(o => o.EmpleadoResponsable)
            .Include(o => o.ResultadosClave)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (objetivo is null)
        {
            return NotFound();
        }

        OkrService.ActualizarObjetivo(objetivo);
        await _context.SaveChangesAsync();

        Objetivo = objetivo;

        await CargarCombosAsync(
            Objetivo.SectorId,
            Objetivo.PuestoId,
            Objetivo.EmpleadoResponsableId
        );

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidarObjetivoAsync();

        if (!ModelState.IsValid)
        {
            await CargarCombosAsync(
                Objetivo.SectorId,
                Objetivo.PuestoId,
                Objetivo.EmpleadoResponsableId
            );

            return Page();
        }

        var objetivoDb = await _context.ObjetivosOkr
            .Include(o => o.ResultadosClave)
            .FirstOrDefaultAsync(o => o.Id == Objetivo.Id);

        if (objetivoDb is null)
        {
            return NotFound();
        }

        objetivoDb.Titulo = Objetivo.Titulo;
        objetivoDb.Descripcion = Objetivo.Descripcion;
        objetivoDb.SectorId = Objetivo.SectorId;
        objetivoDb.PuestoId = Objetivo.PuestoId;
        objetivoDb.EmpleadoResponsableId = Objetivo.EmpleadoResponsableId;
        objetivoDb.FechaInicio = Objetivo.FechaInicio;
        objetivoDb.FechaFin = Objetivo.FechaFin;
        objetivoDb.Activo = Objetivo.Activo;

        OkrService.ActualizarObjetivo(objetivoDb);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Details", new { id = objetivoDb.Id });
    }

    private async Task ValidarObjetivoAsync()
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

        if (Objetivo.SectorId != null && Objetivo.PuestoId != null)
        {
            var puestoPerteneceAlSector = await _context.Puestos
                .AnyAsync(p =>
                    p.Id == Objetivo.PuestoId &&
                    p.SectorId == Objetivo.SectorId &&
                    p.Activo);

            if (!puestoPerteneceAlSector)
            {
                ModelState.AddModelError("Objetivo.PuestoId", "El puesto seleccionado no pertenece al sector indicado.");
            }
        }

        if (Objetivo.SectorId != null && Objetivo.EmpleadoResponsableId != null)
        {
            var empleadoValido = await _context.Empleados
                .AnyAsync(e =>
                    e.Id == Objetivo.EmpleadoResponsableId &&
                    e.SectorId == Objetivo.SectorId &&
                    e.Activo &&
                    (Objetivo.PuestoId == null || e.PuestoId == Objetivo.PuestoId));

            if (!empleadoValido)
            {
                ModelState.AddModelError("Objetivo.EmpleadoResponsableId", "El responsable seleccionado no corresponde al sector o puesto indicado.");
            }
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