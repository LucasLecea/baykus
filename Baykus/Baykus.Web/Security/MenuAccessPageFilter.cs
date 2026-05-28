using Baykus.Web.Data;
using Baykus.Web.Services.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Baykus.Web.Security
{
    public class MenuAccessPageFilter : IAsyncPageFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMenuUsuarioService _menuUsuarioService;

        public MenuAccessPageFilter(
            UserManager<ApplicationUser> userManager,
            IMenuUsuarioService menuUsuarioService)
        {
            _userManager = userManager;
            _menuUsuarioService = menuUsuarioService;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(
            PageHandlerExecutingContext context,
            PageHandlerExecutionDelegate next)
        {

            //throw new Exception("El filtro de seguridad se está ejecutando");
            var user = context.HttpContext.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                await next();
                return;
            }

            var pagePath = context.ActionDescriptor.ViewEnginePath;
            var area = context.ActionDescriptor.AreaName;

            if (EsPaginaLibre(pagePath, area))
            {
                await next();
                return;
            }

            var applicationUserId = _userManager.GetUserId(user);

            if (string.IsNullOrWhiteSpace(applicationUserId))
            {
                context.Result = new RedirectToPageResult("/AccesoDenegado");
                return;
            }

            var tieneAcceso = await _menuUsuarioService.UsuarioTieneAccesoAsync(
                applicationUserId,
                pagePath
            );

            if (!tieneAcceso)
            {
                context.Result = new RedirectToPageResult("/AccesoDenegado");
                return;
            }

            await next();
        }

        private static bool EsPaginaLibre(string? pagePath, string? area)
        {
            if (!string.IsNullOrWhiteSpace(area) &&
                area.Equals("Identity", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(pagePath))
            {
                return false;
            }

            pagePath = pagePath.ToLower();

            return pagePath == "/accesodenegado" ||
                   pagePath == "/error";
        }
    }
}
