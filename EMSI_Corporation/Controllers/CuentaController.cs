using Microsoft.AspNetCore.Mvc;
using EMSI_Corporation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EMSI_Corporation.Models;
using Microsoft.AspNetCore.Identity;

namespace EMSI_Corporation.Controllers
{
    public class CuentaController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public CuentaController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Perfil()
        {
            // Obtiene el ID del usuario autenticado
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Incluye datos relacionados: Empleado y Rol
            var usuario = _appDBContext.usuarios
                .Include(u => u.empleado)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefault(u => u.IdUsuario == usuarioId);

            if (usuario == null)
                return NotFound();

            ViewBag.Rol = usuario.UserRoles.FirstOrDefault()?.Rol?.nomRol ?? "Sin rol asignado";
            return View("Perfil", usuario);
        }
    }
}
