using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.EmpleadoPages;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Empleado Empleado { get; set; } = new();

    public SelectList PuestosSelectList { get; set; } = default!;
    public SelectList SectoresSelectList { get; set; } = default!;

    public string PuestosJson { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync()
    {
        await CargarCombosAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Empleado.SectorId == null)
        {
            ModelState.AddModelError("Empleado.SectorId", "Debe seleccionar un sector.");
        }

        if (Empleado.PuestoId == null)
        {
            ModelState.AddModelError("Empleado.PuestoId", "Debe seleccionar un puesto.");
        }

        if (Empleado.SectorId != null && Empleado.PuestoId != null)
        {
            var puestoPerteneceAlSector = await _context.Puestos
                .AnyAsync(p =>
                    p.Id == Empleado.PuestoId &&
                    p.SectorId == Empleado.SectorId &&
                    p.Activo);

            if (!puestoPerteneceAlSector)
            {
                ModelState.AddModelError("Empleado.PuestoId", "El puesto seleccionado no pertenece al sector indicado.");
            }
        }

        if (!ModelState.IsValid)
        {
            await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
            return Page();
        }

        Empleado.FechaAlta = DateTime.Now;

        _context.Empleados.Add(Empleado);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task CargarCombosAsync(int? sectorSeleccionadoId = null, int? puestoSeleccionadoId = null)
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

        SectoresSelectList = new SelectList(sectores, "Id", "Nombre", sectorSeleccionadoId);

        var puestosFiltrados = puestos
            .Where(p => p.sectorId == sectorSeleccionadoId)
            .ToList();

        PuestosSelectList = new SelectList(puestosFiltrados, "id", "nombre", puestoSeleccionadoId);

        PuestosJson = JsonSerializer.Serialize(puestos);
    }
}