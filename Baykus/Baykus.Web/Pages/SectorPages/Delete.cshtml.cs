using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.SectorPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Sector Sector { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var sector = await _context.Sector.FirstOrDefaultAsync(m => m.Id == id);
        if (sector is null)
        {
            return NotFound();
        }
        else
        {
            Sector = sector;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var sector = await _context.Sector.FindAsync(id);
        if (sector != null)
        {
            Sector = sector;
            _context.Sector.Remove(Sector);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
