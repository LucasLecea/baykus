using Baykus.Web.Models;

namespace Baykus.Web.Services
{
    public class OkrService
    {
        public static void ActualizarEstadoResultado(ResultadoClaveOkr resultado)
        {
            if (resultado.Progreso <= 0)
            {
                resultado.Progreso = 0;
                resultado.Estado = "Pendiente";
            }
            else if (resultado.Progreso >= 100)
            {
                resultado.Progreso = 100;
                resultado.Estado = "Completado";
            }
            else
            {
                resultado.Estado = "En progreso";
            }
        }

        public static void ActualizarObjetivo(ObjetivoOkr objetivo)
        {
            if (objetivo.ResultadosClave == null || !objetivo.ResultadosClave.Any())
            {
                objetivo.Progreso = 0;
                objetivo.Estado = objetivo.FechaFin.Date < DateTime.Today ? "Vencido" : "Pendiente";
                return;
            }

            objetivo.Progreso = Convert.ToInt32(objetivo.ResultadosClave.Average(r => r.Progreso));

            if (objetivo.Progreso >= 100)
            {
                objetivo.Progreso = 100;
                objetivo.Estado = "Completado";
            }
            else if (objetivo.FechaFin.Date < DateTime.Today)
            {
                objetivo.Estado = "Vencido";
            }
            else if (objetivo.Progreso > 0)
            {
                objetivo.Estado = "En progreso";
            }
            else
            {
                objetivo.Estado = "Pendiente";
            }
        }

        public static string ObtenerClaseEstado(string estado)
        {
            return estado switch
            {
                "Completado" => "baykus-badge-success",
                "En progreso" => "baykus-badge-info",
                "Vencido" => "baykus-badge-danger",
                "Pendiente" => "baykus-badge-muted",
                _ => "baykus-badge-muted"
            };
        }

        public static string ObtenerClaseProgreso(int progreso)
        {
            return progreso switch
            {
                >= 80 => "bg-success",
                >= 50 => "bg-info",
                >= 25 => "bg-warning",
                _ => "bg-danger"
            };
        }
    }
}
