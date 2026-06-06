using Baykus.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Baykus.Web.Services.Menu
{
    public class MenuUsuarioService : IMenuUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public MenuUsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuUsuarioDto>> ObtenerMenuUsuarioAsync(string applicationUserId)
        {
            var menus = await (
                from empleado in _context.Empleados.AsNoTracking()
                join empleadoPerfil in _context.EmpleadoPerfiles.AsNoTracking()
                    on empleado.Id equals empleadoPerfil.EmpleadoId
                join perfil in _context.Perfiles.AsNoTracking()
                    on empleadoPerfil.PerfilId equals perfil.Id
                join permiso in _context.PerfilMenuPermisos.AsNoTracking()
                    on perfil.Id equals permiso.PerfilId
                join menu in _context.MenusSistema.AsNoTracking()
                    on permiso.MenuSistemaId equals menu.Id
                where empleado.ApplicationUserId == applicationUserId
                      && empleado.Activo
                      && empleadoPerfil.Activo
                      && perfil.Activo
                      && menu.Activo
                      && menu.EsVisible
                      && permiso.PuedeVer
                select new MenuUsuarioDto
                {
                    Id = menu.Id,
                    MenuPadreId = menu.MenuPadreId,
                    Nombre = menu.Nombre,
                    Codigo = menu.Codigo,
                    Icono = menu.Icono,
                    Url = menu.Url,
                    Page = menu.Page,
                    Area = menu.Area,
                    Orden = menu.Orden,

                    PuedeVer = permiso.PuedeVer,
                    PuedeCrear = permiso.PuedeCrear,
                    PuedeEditar = permiso.PuedeEditar,
                    PuedeEliminar = permiso.PuedeEliminar,
                    PuedeAprobar = permiso.PuedeAprobar,
                    PuedeRevisar = permiso.PuedeRevisar
                })
                .ToListAsync();

            var menusUnicos = menus
                .GroupBy(m => m.Id)
                .Select(g => new MenuUsuarioDto
                {
                    Id = g.Key,
                    MenuPadreId = g.First().MenuPadreId,
                    Nombre = g.First().Nombre,
                    Codigo = g.First().Codigo,
                    Icono = g.First().Icono,
                    Url = g.First().Url,
                    Page = g.First().Page,
                    Area = g.First().Area,
                    Orden = g.First().Orden,

                    PuedeVer = g.Any(x => x.PuedeVer),
                    PuedeCrear = g.Any(x => x.PuedeCrear),
                    PuedeEditar = g.Any(x => x.PuedeEditar),
                    PuedeEliminar = g.Any(x => x.PuedeEliminar),
                    PuedeAprobar = g.Any(x => x.PuedeAprobar),
                    PuedeRevisar = g.Any(x => x.PuedeRevisar)
                })
                .OrderBy(m => m.Orden)
                .ToList();

            var idsPadresFaltantes = menusUnicos
                .Where(m => m.MenuPadreId != null)
                .Select(m => m.MenuPadreId!.Value)
                .Distinct()
                .Where(idPadre => !menusUnicos.Any(m => m.Id == idPadre))
                .ToList();

            if (idsPadresFaltantes.Any())
            {
                var padresFaltantes = await _context.MenusSistema
                    .AsNoTracking()
                    .Where(m =>
                        idsPadresFaltantes.Contains(m.Id) &&
                        m.Activo &&
                        m.EsVisible)
                    .Select(m => new MenuUsuarioDto
                    {
                        Id = m.Id,
                        MenuPadreId = m.MenuPadreId,
                        Nombre = m.Nombre,
                        Codigo = m.Codigo,
                        Icono = m.Icono,
                        Url = m.Url,
                        Page = m.Page,
                        Area = m.Area,
                        Orden = m.Orden,

                        PuedeVer = true,
                        PuedeCrear = false,
                        PuedeEditar = false,
                        PuedeEliminar = false,
                        PuedeAprobar = false,
                        PuedeRevisar = false
                    })
                    .ToListAsync();

                menusUnicos.AddRange(padresFaltantes);

                menusUnicos = menusUnicos
                    .OrderBy(m => m.Orden)
                    .ToList();
            }

            var padres = menusUnicos
                .Where(m => m.MenuPadreId == null)
                .OrderBy(m => m.Orden)
                .ToList();

            foreach (var padre in padres)
            {
                padre.Hijos = menusUnicos
                    .Where(m => m.MenuPadreId == padre.Id)
                    .OrderBy(m => m.Orden)
                    .ToList();
            }

            padres = padres
                .Where(m => !string.IsNullOrWhiteSpace(m.Page)
                         || !string.IsNullOrWhiteSpace(m.Url)
                         || m.Hijos.Any())
                .ToList();

            return padres;
        }

        public async Task<bool> UsuarioTieneAccesoAsync(string applicationUserId, string pagePath)
        {
            if (string.IsNullOrWhiteSpace(applicationUserId) ||
                string.IsNullOrWhiteSpace(pagePath))
            {
                return false;
            }

            pagePath = NormalizarPath(pagePath);

            var paginasPermitidas = await (
                from empleado in _context.Empleados.AsNoTracking()
                join empleadoPerfil in _context.EmpleadoPerfiles.AsNoTracking()
                    on empleado.Id equals empleadoPerfil.EmpleadoId
                join perfil in _context.Perfiles.AsNoTracking()
                    on empleadoPerfil.PerfilId equals perfil.Id
                join permiso in _context.PerfilMenuPermisos.AsNoTracking()
                    on perfil.Id equals permiso.PerfilId
                join menu in _context.MenusSistema.AsNoTracking()
                    on permiso.MenuSistemaId equals menu.Id
                where empleado.ApplicationUserId == applicationUserId
                      && empleado.Activo
                      && empleadoPerfil.Activo
                      && perfil.Activo
                      && menu.Activo
                      && permiso.PuedeVer
                      && menu.Page != null
                select menu.Page
            ).ToListAsync();

            return paginasPermitidas.Any(p =>
            {
                var menuPage = NormalizarPath(p);

                if (string.IsNullOrWhiteSpace(menuPage))
                {
                    return false;
                }

                return pagePath == menuPage ||
                       pagePath.StartsWith(menuPage + "/");
            });
        }

        private static string NormalizarPath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            path = path.Trim().ToLower();

            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            /*
             * Caso especial:
             * /Index es el dashboard raíz.
             * No debe convertirse en vacío, porque si queda vacío habilita todo.
             */
            if (path == "/index")
            {
                return "/index";
            }

            /*
             * Para módulos sí normalizamos:
             * /EmpleadosPages/Index => /empleadospages
             * /OkrPages/Index       => /okrpages
             */
            if (path.EndsWith("/index"))
            {
                path = path[..^6];
            }

            return path;
        }
    }
}
