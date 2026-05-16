using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public Empleado Empleado { get; set; } = default!;

    public SelectList PuestosSelectList { get; set; } = default!;
    public SelectList SectoresSelectList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
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

        Empleado.FechaAlta = DateTime.Now;

        _context.Empleados.Add(Empleado);
        await _context.SaveChangesAsync();

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

        PuestosSelectList = new SelectList(puestos, "Id", "Nombre");
        SectoresSelectList = new SelectList(sectores, "Id", "Nombre");
    }
}