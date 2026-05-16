using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.EmpleadoPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Empleado Empleado { get; set; } = default!;

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

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var empleado = await _context.Empleados.FindAsync(id);

        if (empleado is not null)
        {
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
