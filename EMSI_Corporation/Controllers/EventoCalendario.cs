using EMSI_Corporation.Data;
using EMSI_Corporation.Models;
using EMSI_Corporation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EMSI_Corporation.Controllers
{
    [Authorize]
    public class CalendarioController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public CalendarioController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public async Task<IActionResult> Index(UsuarioEventosVM usev)
        {
            var eventos = await _appDBContext.EventosCalendario
                .Include(e => e.Empleado)
                .ToListAsync();

            if (AccesoController.user_id == -1)
            {
                Console.WriteLine("-----AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                Console.WriteLine("Usuario no encontrado");
            }
            UsuarioEventosVM nvo = new UsuarioEventosVM()
            {
                eventos = eventos,
                usuario = _appDBContext.usuarios.Include(e => e.UserRoles).Where(r => r.IdUsuario == AccesoController.user_id).ToList()[0],
            };
            return View(nvo);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            var usuario = _appDBContext.usuarios.Include(e => e.UserRoles).Where(r => r.IdUsuario == AccesoController.user_id).ToList()[0];
            if (usuario == null || usuario.UserRoles.ToList()[0].IdRol == 1)
            {
                return View(new EventoCalendarioVM
                {
                    FechaInicio = DateTime.Now
                .AddMinutes(1)
                .AddSeconds(-DateTime.Now.Second)
                .AddMilliseconds(-DateTime.Now.Millisecond),

                    FechaFin = DateTime.Now
                .AddMinutes(30)
                .AddSeconds(-DateTime.Now.Second)
                .AddMilliseconds(-DateTime.Now.Millisecond)
                });
            }
            else
            {
                var eventos = _appDBContext.EventosCalendario
                .Include(e => e.Empleado)
                .ToList();
                UsuarioEventosVM nvo = new UsuarioEventosVM()
                {
                    eventos = eventos,
                    usuario = usuario
                };
                return View("Index", nvo);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(EventoCalendarioVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int usuarioId))
            {
                ModelState.AddModelError("", "No se pudo obtener el usuario actual.");
                return View(model);
            }

            var usuario = await _appDBContext.usuarios
                .Include(u => u.empleado)
                .FirstOrDefaultAsync(u => u.IdUsuario == usuarioId);

            if (usuario?.empleado == null)
            {
                ModelState.AddModelError("", "No se encontró el empleado relacionado al usuario.");
                return View(model);
            }

            var nuevoEvento = new EventoCalendario
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                Color = model.Color,
                EmpleadoId = usuario.empleado.IdEmpleado
            };

            _appDBContext.EventosCalendario.Add(nuevoEvento);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
                return NotFound();

            var evento = await _appDBContext.EventosCalendario.FindAsync(id);
            if (evento == null)
                return NotFound();

            var eventos = await _appDBContext.EventosCalendario
                .Include(e => e.Empleado)
                .ToListAsync();

            if (AccesoController.user_id == -1)
            {
                return NotFound();
            }
            UsuarioEventosVM nvo = new UsuarioEventosVM()
            {
                eventos = eventos,
                usuario = _appDBContext.usuarios.Include(e => e.UserRoles).Where(r => r.IdUsuario == AccesoController.user_id).ToList()[0],
            };

            var vm = new EventoCalendarioVM
            {
                IdEvento = evento.IdEvento,
                Titulo = evento.Titulo,
                Descripcion = evento.Descripcion,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin,
                Color = evento.Color
            };
            ViewBag.nvo = nvo;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, EventoCalendarioVM model)
        {
            if (id != model.IdEvento)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var eventoExistente = await _appDBContext.EventosCalendario.FindAsync(id);
                if (eventoExistente == null)
                    return NotFound();

                // Solo actualiza los campos permitidos
                eventoExistente.Titulo = model.Titulo;
                eventoExistente.Descripcion = model.Descripcion;
                eventoExistente.FechaInicio = model.FechaInicio;
                eventoExistente.FechaFin = model.FechaFin;
                eventoExistente.Color = model.Color;
                // No toques EmpleadoId

                await _appDBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_appDBContext.EventosCalendario.Any(e => e.IdEvento == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
                return NotFound();

            var evento = await _appDBContext.EventosCalendario
                .Include(e => e.Empleado)
                .FirstOrDefaultAsync(e => e.IdEvento == id);

            if (evento == null)
                return NotFound();

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            var evento = await _appDBContext.EventosCalendario.FindAsync(id);
            if (evento != null)
            {
                _appDBContext.EventosCalendario.Remove(evento);
                await _appDBContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ObtenerEventos()
        {
            var eventos = _appDBContext.EventosCalendario
                .AsEnumerable()
                .Select(e => new
                {
                    id = e.IdEvento,
                    title = e.Titulo,
                    start = e.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = e.FechaFin?.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = e.Color
                })
                .ToList();

            return Json(eventos);
        }


    }
}
