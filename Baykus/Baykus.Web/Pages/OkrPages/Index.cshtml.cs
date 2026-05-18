using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Data;
using Baykus.Web.Models;
using Baykus.Web.Services;

namespace Baykus.Web.Pages.OkrPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<ObjetivoOkr> Objetivos { get; set; } = new List<ObjetivoOkr>();

    public SelectList SectoresSelectList { get; set; } = default!;

    public int? SectorId { get; set; }
    public string? Estado { get; set; }

    public async Task OnGetAsync(int? sectorId, string? estado)
    {
        SectorId = sectorId;
        Estado = estado;

        var query = _context.ObjetivosOkr
            .Include(o => o.Sector)
            .Include(o => o.Puesto)
            .Include(o => o.EmpleadoResponsable)
            .Include(o => o.ResultadosClave)
            .AsQueryable();

        if (sectorId.HasValue)
        {
            query = query.Where(o => o.SectorId == sectorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(estado))
        {
            query = query.Where(o => o.Estado == estado);
        }

        Objetivos = await query
            .OrderBy(o => o.FechaFin)
            .ToListAsync();

        foreach (var objetivo in Objetivos)
        {
            OkrService.ActualizarObjetivo(objetivo);
        }

        await _context.SaveChangesAsync();

        var sectores = await _context.Sector
            .AsNoTracking()
            .Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        SectoresSelectList = new SelectList(sectores, "Id", "Nombre", sectorId);
    }
}