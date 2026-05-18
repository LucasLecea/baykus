using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.PuestoPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Puesto Puesto { get; set; } = default!;

    public SelectList Sectores { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var puesto = await _context.Puestos
            .FirstOrDefaultAsync(m => m.Id == id);

        if (puesto is null)
        {
            return NotFound();
        }

        Puesto = puesto;

        await CargarSectoresAsync(Puesto.SectorId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Puesto.SectorId is null)
        {
            ModelState.AddModelError("Puesto.SectorId", "Debe seleccionar un sector.");
        }

        if (!ModelState.IsValid)
        {
            await CargarSectoresAsync(Puesto.SectorId);
            return Page();
        }

        _context.Attach(Puesto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PuestoExists(Puesto.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private async Task CargarSectoresAsync(int? sectorSeleccionadoId = null)
    {
        var sectores = await _context.Sector
            .Where(x => x.Activo != false)
            .AsNoTracking()
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        Sectores = new SelectList(sectores, "Id", "Nombre", sectorSeleccionadoId);
    }

    private bool PuestoExists(int id)
    {
        return _context.Puestos.Any(e => e.Id == id);
    }
}