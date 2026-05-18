using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Data;
using Baykus.Web.Models;

namespace Baykus.Web.Pages.OkrPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public ObjetivoOkr Objetivo { get; set; } = default!;

    public int CantidadResultadosClave { get; set; }
    public int CantidadSeguimientos { get; set; }

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
            .Include(o => o.Seguimientos)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (objetivo is null)
        {
            return NotFound();
        }

        Objetivo = objetivo;
        CantidadResultadosClave = objetivo.ResultadosClave.Count;
        CantidadSeguimientos = objetivo.Seguimientos.Count;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var objetivo = await _context.ObjetivosOkr
            .FirstOrDefaultAsync(o => o.Id == id);

        if (objetivo is null)
        {
            return NotFound();
        }

        _context.ObjetivosOkr.Remove(objetivo);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}