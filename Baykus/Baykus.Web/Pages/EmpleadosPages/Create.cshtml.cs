using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Baykus.Web.Models;
using Baykus.Web.Data;

namespace Baykus.Web.Pages.EmpleadoPages;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateModel(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public Empleado Empleado { get; set; } = new();

    [BindProperty]
    public int? PerfilSeleccionadoId { get; set; }

    [BindProperty]
    public bool CrearUsuarioAcceso { get; set; }

    public SelectList PuestosSelectList { get; set; } = default!;
    public SelectList SectoresSelectList { get; set; } = default!;
    public SelectList PerfilesSelectList { get; set; } = default!;

    public string PuestosJson { get; set; } = "[]";

    [TempData]
    public string? PasswordTemporal { get; set; }

    [TempData]
    public string? MensajeOk { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Empleado.Activo = true;

        await CargarCombosAsync();
        await CargarPerfilesAsync();

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

        if (CrearUsuarioAcceso && string.IsNullOrWhiteSpace(Empleado.Email))
        {
            ModelState.AddModelError("Empleado.Email", "Debe cargar un email para crear el usuario de acceso.");
        }

        if (CrearUsuarioAcceso && PerfilSeleccionadoId == null)
        {
            ModelState.AddModelError("PerfilSeleccionadoId", "Debe seleccionar un perfil para crear el usuario de acceso.");
        }

        if (PerfilSeleccionadoId.HasValue)
        {
            var perfilExiste = await _context.Perfiles
                .AnyAsync(p => p.Id == PerfilSeleccionadoId.Value && p.Activo);

            if (!perfilExiste)
            {
                ModelState.AddModelError("PerfilSeleccionadoId", "El perfil seleccionado no existe o está inactivo.");
            }
        }

        if (!string.IsNullOrWhiteSpace(Empleado.Email))
        {
            var emailNormalizado = Empleado.Email.Trim();

            var existeEmpleadoConEmail = await _context.Empleados
                .AnyAsync(e => e.Email == emailNormalizado);

            if (existeEmpleadoConEmail)
            {
                ModelState.AddModelError("Empleado.Email", "Ya existe un empleado con este email.");
            }

            if (CrearUsuarioAcceso)
            {
                var usuarioExistente = await _userManager.FindByEmailAsync(emailNormalizado);

                if (usuarioExistente is not null)
                {
                    ModelState.AddModelError("Empleado.Email", "Ya existe un usuario de acceso con este email.");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
            await CargarPerfilesAsync(PerfilSeleccionadoId);

            return Page();
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            Empleado.FechaAlta = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(Empleado.Email))
            {
                Empleado.Email = Empleado.Email.Trim();
            }

            if (CrearUsuarioAcceso)
            {
                var passwordTemporal = GenerarPasswordTemporal();

                var nuevoUsuario = new ApplicationUser
                {
                    UserName = Empleado.Email,
                    Email = Empleado.Email,
                    EmailConfirmed = true
                };

                var resultado = await _userManager.CreateAsync(nuevoUsuario, passwordTemporal);

                if (!resultado.Succeeded)
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    await CargarCombosAsync(Empleado.SectorId, Empleado.PuestoId);
                    await CargarPerfilesAsync(PerfilSeleccionadoId);

                    return Page();
                }

                Empleado.ApplicationUserId = nuevoUsuario.Id;
                PasswordTemporal = passwordTemporal;
            }

            _context.Empleados.Add(Empleado);
            await _context.SaveChangesAsync();

            if (PerfilSeleccionadoId.HasValue)
            {
                _context.EmpleadoPerfiles.Add(new EmpleadoPerfil
                {
                    EmpleadoId = Empleado.Id,
                    PerfilId = PerfilSeleccionadoId.Value,
                    Activo = true,
                    FechaAsignacion = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            if (CrearUsuarioAcceso)
            {
                MensajeOk = "Empleado creado correctamente con usuario de acceso.";
                return RedirectToPage("./Edit", new { id = Empleado.Id });
            }

            MensajeOk = "Empleado creado correctamente.";
            return RedirectToPage("./Index");
        }
        catch
        {
            await transaction.RollbackAsync();
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

    private async Task CargarPerfilesAsync(int? perfilSeleccionadoId = null)
    {
        var perfiles = await _context.Perfiles
            .AsNoTracking()
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        PerfilesSelectList = new SelectList(perfiles, "Id", "Nombre", perfilSeleccionadoId);
    }

    private static string GenerarPasswordTemporal()
    {
        return $"Baykus#{DateTime.Now:yyyyMMddHHmmss}";
    }
}