using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.PuestoPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Puesto Puesto { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var puesto = await _context.Puestos.FirstOrDefaultAsync(m => m.Id == id);
        if (puesto is null)
        {
            return NotFound();
        }
        else
        {
            Puesto = puesto;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var puesto = await _context.Puestos.FindAsync(id);
        if (puesto != null)
        {
            Puesto = puesto;
            _context.Puestos.Remove(Puesto);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
