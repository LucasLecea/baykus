using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.PuestoPages;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Puesto Puesto { get; set; } = new();

    public SelectList Sectores { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        await CargarSectoresAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Puesto.SectorId == null)
        {
            ModelState.AddModelError("Puesto.SectorId", "Debe seleccionar un sector.");
        }

        if (!ModelState.IsValid)
        {
            await CargarSectoresAsync();
            return Page();
        }

        _context.Puestos.Add(Puesto);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task CargarSectoresAsync()
    {
        var sectores = await _context.Sector
            .AsNoTracking()
            .Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        Sectores = new SelectList(sectores, "Id", "Nombre");
    }
}