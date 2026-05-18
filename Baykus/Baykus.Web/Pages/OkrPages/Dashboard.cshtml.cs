using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Baykus.Web.Data;
using Baykus.Web.Models;
using Baykus.Web.Services;

namespace Baykus.Web.Pages.OkrPages;

public class DashboardModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DashboardModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int TotalObjetivos { get; set; }
    public int ObjetivosPendientes { get; set; }
    public int ObjetivosEnProgreso { get; set; }
    public int ObjetivosCompletados { get; set; }
    public int ObjetivosVencidos { get; set; }
    public int AvancePromedio { get; set; }

    public IList<ObjetivoOkr> ProximosAVencer { get; set; } = new List<ObjetivoOkr>();
    public IList<ObjetivoOkr> ObjetivosCriticos { get; set; } = new List<ObjetivoOkr>();
    public IList<ObjetivosPorSectorVm> ObjetivosPorSector { get; set; } = new List<ObjetivosPorSectorVm>();
    public IList<ObjetivoOkr> UltimosObjetivos { get; set; } = new List<ObjetivoOkr>();

    public async Task OnGetAsync()
    {
        var objetivos = await _context.ObjetivosOkr
            .Include(o => o.Sector)
            .Include(o => o.Puesto)
            .Include(o => o.EmpleadoResponsable)
            .Include(o => o.ResultadosClave)
            .Where(o => o.Activo)
            .ToListAsync();

        foreach (var objetivo in objetivos)
        {
            OkrService.ActualizarObjetivo(objetivo);
        }

        await _context.SaveChangesAsync();

        TotalObjetivos = objetivos.Count;
        ObjetivosPendientes = objetivos.Count(o => o.Estado == "Pendiente");
        ObjetivosEnProgreso = objetivos.Count(o => o.Estado == "En progreso");
        ObjetivosCompletados = objetivos.Count(o => o.Estado == "Completado");
        ObjetivosVencidos = objetivos.Count(o => o.Estado == "Vencido");

        AvancePromedio = TotalObjetivos > 0
            ? Convert.ToInt32(objetivos.Average(o => o.Progreso))
            : 0;

        var hoy = DateTime.Today;
        var limiteProximos = hoy.AddDays(15);

        ProximosAVencer = objetivos
            .Where(o =>
                o.Estado != "Completado" &&
                o.FechaFin.Date >= hoy &&
                o.FechaFin.Date <= limiteProximos)
            .OrderBy(o => o.FechaFin)
            .Take(5)
            .ToList();

        ObjetivosCriticos = objetivos
            .Where(o =>
                o.Estado == "Vencido" ||
                (o.Estado != "Completado" && o.FechaFin.Date <= limiteProximos && o.Progreso < 50))
            .OrderBy(o => o.FechaFin)
            .Take(6)
            .ToList();

        ObjetivosPorSector = objetivos
            .GroupBy(o => o.Sector?.Nombre ?? "Sin sector")
            .Select(g => new ObjetivosPorSectorVm
            {
                Sector = g.Key,
                Total = g.Count(),
                Completados = g.Count(o => o.Estado == "Completado"),
                EnProgreso = g.Count(o => o.Estado == "En progreso"),
                Vencidos = g.Count(o => o.Estado == "Vencido"),
                AvancePromedio = g.Any()
                    ? Convert.ToInt32(g.Average(o => o.Progreso))
                    : 0
            })
            .OrderByDescending(x => x.Total)
            .ToList();

        UltimosObjetivos = objetivos
            .OrderByDescending(o => o.FechaCreacion)
            .Take(5)
            .ToList();
    }

    public class ObjetivosPorSectorVm
    {
        public string Sector { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Completados { get; set; }
        public int EnProgreso { get; set; }
        public int Vencidos { get; set; }
        public int AvancePromedio { get; set; }
    }
}