namespace Baykus.Web.Services.Menu
{
    public interface IMenuUsuarioService
    {
        Task<List<MenuUsuarioDto>> ObtenerMenuUsuarioAsync(string applicationUserId);
        Task<bool> UsuarioTieneAccesoAsync(string applicationUserId, string pagePath);
    }
}
