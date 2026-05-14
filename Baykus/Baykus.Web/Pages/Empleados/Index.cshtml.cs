using Baykus.Web.Data;
using Baykus.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Baykus.Web.Pages.Empleados;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Empleado> Empleados { get; set; } = new List<Empleado>();

    public async Task OnGetAsync()
    {
        Empleados = await _context.Empleados
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .ToListAsync();
    }
}