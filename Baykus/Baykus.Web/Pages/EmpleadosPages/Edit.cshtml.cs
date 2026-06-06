using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.EmpleadoPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditModel(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public Empleado Empleado { get; set; } = default!;

    [BindProperty]
    public List<int> PerfilesSeleccionadosIds { get; set; } = new();

    [BindProperty]
    public bool CrearUsuarioAcceso { get; set; }

    public SelectList PuestosSelectList { get; set; } = default!;
    public SelectList SectoresSelectList { get; set; } = default!;
    public MultiSelectList PerfilesSelectList { get; set; } = default!;

    public string PuestosJson { get; set; } = "[]";

    public bool TieneUsuarioAcceso { get; set; }
    public string? UsuarioAccesoEmail { get; set; }

    [TempData]
    public string? MensajeOk { get; set; }

    [TempData]
    public string? PasswordTemporal { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var empleado = await _context.Empleados
            .Include(e => e.Puesto)
            .Include(e => e.Sector)
            .Include(e => e.ApplicationUser)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (empleado is null)
        {
            return NotFound();
        }

        Empleado = empleado;

        PerfilesSeleccionadosIds = await _context.EmpleadoPerfiles
            .Where(ep => ep.EmpleadoId == Empleado.Id && ep.Activo)
            .Select(ep => ep.PerfilId)
            .ToListAsync();

        TieneUsuarioAcceso = !string.IsNullOrWhiteSpace(Empleado.ApplicationUserId);
        UsuarioAccesoEmail = Empleado.ApplicationUser?.Email;

        await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
        await CargarPerfilesAsync(PerfilesSeleccionadosIds);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Empleado.SectorId == null)
        {
            ModelState.AddModelError("Empleado.SectorId", "Debe seleccionar un sector.");
        }

        if (Empleado.PuestoId == null)
        {
            ModelState.AddModelError("Empleado.PuestoId", "Debe seleccionar un puesto.");
        }

        if (Empleado.SectorId != null && Empleado.PuestoId != null)
        {
            var puestoPerteneceAlSector = await _context.Puestos
                .AnyAsync(p =>
                    p.Id == Empleado.PuestoId &&
                    p.SectorId == Empleado.SectorId &&
                    p.Activo);

            if (!puestoPerteneceAlSector)
            {
                ModelState.AddModelError("Empleado.PuestoId", "El puesto seleccionado no pertenece al sector indicado.");
            }
        }

        if (CrearUsuarioAcceso && !PerfilesSeleccionadosIds.Any())
        {
            ModelState.AddModelError("PerfilesSeleccionadosIds", "Debe seleccionar al menos un perfil para crear el usuario de acceso.");
        }

        if (CrearUsuarioAcceso && string.IsNullOrWhiteSpace(Empleado.Email))
        {
            ModelState.AddModelError("Empleado.Email", "Debe cargar un email para crear el usuario de acceso.");
        }

        var empleadoDb = await _context.Empleados
            .Include(e => e.ApplicationUser)
            .FirstOrDefaultAsync(e => e.Id == Empleado.Id);

        if (empleadoDb is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            Empleado.ApplicationUserId = empleadoDb.ApplicationUserId;

            TieneUsuarioAcceso = !string.IsNullOrWhiteSpace(empleadoDb.ApplicationUserId);
            UsuarioAccesoEmail = empleadoDb.ApplicationUser?.Email;

            await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
            await CargarPerfilesAsync(PerfilesSeleccionadosIds);

            return Page();
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            empleadoDb.Nombre = Empleado.Nombre;
            empleadoDb.Apellido = Empleado.Apellido;
            empleadoDb.Dni = Empleado.Dni;
            empleadoDb.Email = Empleado.Email;
            empleadoDb.Telefono = Empleado.Telefono;
            empleadoDb.SectorId = Empleado.SectorId;
            empleadoDb.PuestoId = Empleado.PuestoId;
            empleadoDb.FechaIngreso = Empleado.FechaIngreso;
            empleadoDb.Activo = Empleado.Activo;

            var perfilesSeleccionadosLimpios = PerfilesSeleccionadosIds.Distinct().ToList();

            if (perfilesSeleccionadosLimpios.Any())
            {
                var perfilesValidosIds = await _context.Perfiles
                    .Where(p => perfilesSeleccionadosLimpios.Contains(p.Id) && p.Activo)
                    .Select(p => p.Id)
                    .ToListAsync();

                if (perfilesValidosIds.Count != perfilesSeleccionadosLimpios.Count)
                {
                    ModelState.AddModelError("PerfilesSeleccionadosIds", "Uno o más perfiles seleccionados no existen o están inactivos.");

                    Empleado.ApplicationUserId = empleadoDb.ApplicationUserId;
                    TieneUsuarioAcceso = !string.IsNullOrWhiteSpace(empleadoDb.ApplicationUserId);
                    UsuarioAccesoEmail = empleadoDb.ApplicationUser?.Email;

                    await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
                    await CargarPerfilesAsync(PerfilesSeleccionadosIds);

                    return Page();
                }
            }

            var relacionesActuales = await _context.EmpleadoPerfiles
                .Where(ep => ep.EmpleadoId == empleadoDb.Id)
                .ToListAsync();

            foreach (var relacionActual in relacionesActuales)
            {
                relacionActual.Activo = perfilesSeleccionadosLimpios.Contains(relacionActual.PerfilId);
            }

            var perfilesActualesIds = relacionesActuales
                .Select(ep => ep.PerfilId)
                .ToList();

            var perfilesNuevosIds = perfilesSeleccionadosLimpios
                .Where(perfilId => !perfilesActualesIds.Contains(perfilId))
                .ToList();

            foreach (var perfilNuevoId in perfilesNuevosIds)
            {
                _context.EmpleadoPerfiles.Add(new EmpleadoPerfil
                {
                    EmpleadoId = empleadoDb.Id,
                    PerfilId = perfilNuevoId,
                    Activo = true,
                    FechaAsignacion = DateTime.Now
                });
            }

            if (CrearUsuarioAcceso && string.IsNullOrWhiteSpace(empleadoDb.ApplicationUserId))
            {
                var emailNormalizado = Empleado.Email!.Trim();

                var usuarioExistente = await _userManager.FindByEmailAsync(emailNormalizado);

                if (usuarioExistente is not null)
                {
                    var usuarioYaVinculado = await _context.Empleados
                        .AnyAsync(e =>
                            e.Id != empleadoDb.Id &&
                            e.ApplicationUserId == usuarioExistente.Id);

                    if (usuarioYaVinculado)
                    {
                        ModelState.AddModelError("Empleado.Email", "Ya existe otro empleado vinculado a un usuario con este email.");

                        Empleado.ApplicationUserId = empleadoDb.ApplicationUserId;
                        TieneUsuarioAcceso = !string.IsNullOrWhiteSpace(empleadoDb.ApplicationUserId);
                        UsuarioAccesoEmail = empleadoDb.ApplicationUser?.Email;

                        await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
                        await CargarPerfilesAsync(PerfilesSeleccionadosIds);

                        return Page();
                    }

                    empleadoDb.ApplicationUserId = usuarioExistente.Id;
                }
                else
                {
                    var passwordTemporal = GenerarPasswordTemporal();

                    var nuevoUsuario = new ApplicationUser
                    {
                        UserName = emailNormalizado,
                        Email = emailNormalizado,
                        EmailConfirmed = true
                    };

                    var resultado = await _userManager.CreateAsync(nuevoUsuario, passwordTemporal);

                    if (!resultado.Succeeded)
                    {
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        Empleado.ApplicationUserId = empleadoDb.ApplicationUserId;
                        TieneUsuarioAcceso = !string.IsNullOrWhiteSpace(empleadoDb.ApplicationUserId);
                        UsuarioAccesoEmail = empleadoDb.ApplicationUser?.Email;

                        await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
                        await CargarPerfilesAsync(PerfilesSeleccionadosIds);

                        return Page();
                    }

                    empleadoDb.ApplicationUserId = nuevoUsuario.Id;
                    PasswordTemporal = passwordTemporal;
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            MensajeOk = "Empleado actualizado correctamente.";

            if (CrearUsuarioAcceso)
            {
                MensajeOk = "Empleado actualizado y usuario de acceso configurado correctamente.";
            }

            return RedirectToPage("./Edit", new { id = empleadoDb.Id });
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();

            if (!EmpleadoExists(Empleado.Id))
            {
                return NotFound();
            }

            throw;
        }
    }

    private async Task CargarCombosAsync(int? sectorSeleccionadoId = null, int? puestoSeleccionadoId = null)
    {
        var sectores = await _context.Sector
            .AsNoTracking()
            .Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        var puestos = await _context.Puestos
            .AsNoTracking()
            .Where(p => p.Activo && p.SectorId != null)
            .OrderBy(p => p.Nombre)
            .Select(p => new
            {
                id = p.Id,
                nombre = p.Nombre,
                sectorId = p.SectorId
            })
            .ToListAsync();

        SectoresSelectList = new SelectList(sectores, "Id", "Nombre", sectorSeleccionadoId);

        var puestosFiltrados = puestos
            .Where(p => p.sectorId == sectorSeleccionadoId)
            .ToList();

        PuestosSelectList = new SelectList(puestosFiltrados, "id", "nombre", puestoSeleccionadoId);

        PuestosJson = JsonSerializer.Serialize(puestos);
    }

    private async Task CargarPerfilesAsync(List<int>? perfilesSeleccionadosIds = null)
    {
        perfilesSeleccionadosIds ??= new List<int>();

        var perfiles = await _context.Perfiles
            .AsNoTracking()
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        PerfilesSelectList = new MultiSelectList(
            perfiles,
            "Id",
            "Nombre",
            perfilesSeleccionadosIds
        );
    }

    private static string GenerarPasswordTemporal()
    {
        return $"Baykus#{DateTime.Now:yyyyMMddHHmmss}";
    }

    private bool EmpleadoExists(int id)
    {
        return _context.Empleados.Any(e => e.Id == id);
    }
}