using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.EmpleadoPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Empleado Empleado { get; set; } = default!;

    public SelectList PuestosSelectList { get; set; } = default!;
    public SelectList SectoresSelectList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var empleado = await _context.Empleados
            .Include(e => e.Puesto)
            .Include(e => e.Sector)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (empleado is null)
        {
            return NotFound();
        }

        Empleado = empleado;

        await CargarCombosAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await CargarCombosAsync();
            return Page();
        }

        _context.Attach(Empleado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmpleadoExists(Empleado.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private async Task CargarCombosAsync()
    {
        var puestos = await _context.Puestos
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        var sectores = await _context.Sector
            .Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        PuestosSelectList = new SelectList(puestos, "Id", "Nombre", Empleado?.PuestoId);
        SectoresSelectList = new SelectList(sectores, "Id", "Nombre", Empleado?.SectorId);
    }

    private bool EmpleadoExists(int id)
    {
        return _context.Empleados.Any(e => e.Id == id);
    }
}