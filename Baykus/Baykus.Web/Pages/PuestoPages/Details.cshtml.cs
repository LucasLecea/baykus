using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.PuestoPages;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Puesto Puesto { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var puesto = await _context.Puestos
            .Include(p => p.Sector)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (puesto is null)
        {
            return NotFound();
        }

        Puesto = puesto;

        return Page();
    }
}