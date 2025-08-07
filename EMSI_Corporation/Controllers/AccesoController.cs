using EMSI_Corporation.Data;
using EMSI_Corporation.Models;
using EMSI_Corporation.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using OfficeOpenXml;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.RegularExpressions;


namespace EMSI_Corporation.Controllers
{
    [Authorize] // Esto asegura que solo usuarios autenticados puedan acceder al resto de acciones
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        private readonly PasswordHasher<Usuario> _passwordHasher;
        public int id_usuario = -1;
        public static int user_id = -1;
        //prueba
        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> IrACalendario()
        {
            var eventos = await _appDBContext.EventosCalendario
                .Include(e => e.Empleado)
                .ToListAsync();
            UsuarioEventosVM nvo = new UsuarioEventosVM()
            {
                eventos = eventos,
                usuario = _appDBContext.usuarios.Find(id_usuario)
            };
            return RedirectToAction("Index", "Calendario", nvo);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var usuario = _appDBContext.usuarios
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefault(u =>
                EF.Functions.Collate(u.usuario, "Latin1_General_CS_AS") == User.Identity.Name);
                user_id = usuario.IdUsuario;
                User.IsInRole("Administrador");
                var a = User.Claims.ElementAt(2).Value;
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            //Valida que los campos contraseñas y usuario esten completados
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            // Validación de caracteres del lado del servidor
            if (!Regex.IsMatch(modelo.Username, @"^[a-zA-Z0-9@._]+$"))
            {
                ModelState.AddModelError(nameof(modelo.Username), "El nombre de usuario contiene caracteres inválidos.");
                return View(modelo);
            }

            var usuario = await _appDBContext.usuarios
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u =>
                EF.Functions.Collate(u.usuario, "Latin1_General_CS_AS") == modelo.Username);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "El usuario no existe.");
                return View(modelo);
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.password, modelo.Password);

            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Contraseña incorrecta.");
                return View(modelo);
            }
            //Si el usuario no tiene rol, asigna rol Atención cliente por defecto
            var rolUsuario = usuario.UserRoles.FirstOrDefault()?.Rol?.nomRol ?? "Empleado";
            id_usuario = usuario.IdUsuario;
            user_id = usuario.IdUsuario;
            //DNI Digital
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.usuario),
                new Claim(ClaimTypes.Role, rolUsuario)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //Recuerda quien eres
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Mantenimiento()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PopupMant()
        {
            ViewBag.Empresas = _appDBContext.Clientes.ToList();
            ViewBag.Trabajadores = _appDBContext.empleados.ToList();
            return View();
        }
        [HttpGet]
        public IActionResult Stakeholders()
        {
            return View();
        }

        // Listar
        [HttpGet]
        public IActionResult Stakeholders_Cliente()
        {
            var lista = _appDBContext.Clientes.ToList();
            return View(lista);
        }

        // Crear
        [HttpGet]
        public IActionResult CrearCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearCliente(Cliente cliente)
        {
            //remover las alertas para que no aparezcan al cargar la página
            TempData.Remove("Error");
            TempData.Remove("Success");

            if (string.IsNullOrWhiteSpace(cliente.NomCliente) || cliente.NomCliente.Length > 100 || int.TryParse(cliente.NomCliente, out _))
            {
                TempData["Error"] = "El Nombre es Inválido, introduce un Nombre válido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.TipoCliente) || cliente.TipoCliente.Length > 16)
            {
                TempData["Error"] = "El TIPO DE CLIENTE es Inválido, selecciona un TIPO DE CLIENTE válido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.TipoDocumento) || cliente.TipoDocumento.Length > 22)
            {
                TempData["Error"] = "El TIPO DE DOCUMENTO es Inválido, selecciona un TIPO DE DOCUMENTO válido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.NumDocumento) || cliente.NumDocumento.Length > 13)
            {
                TempData["Error"] = "El DOCUMENTO es invalido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (cliente.TipoDocumento == "DNI" && cliente.NumDocumento.Length != 8)
            {
                TempData["Error"] = "El DNI debe tener 8 digitos";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (cliente.TipoDocumento == "RUC" && cliente.NumDocumento.Length != 11)
            {
                TempData["Error"] = "El RUC debe tener 12 digitos";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (cliente.TipoDocumento == "CARNET DE EXTRANJERIA" && cliente.NumDocumento.Length != 12)
            {
                TempData["Error"] = "El CARNET DE EXTRANJERIA debe tener 12 digitos";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (_appDBContext.Clientes.Any(C => C.NumDocumento == cliente.NumDocumento))
            {
                TempData["Error"] = "El NÚMERO DE DOCUMENTO ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.NumCelular) || cliente.NumCelular.Length > 20 || !cliente.NumCelular.All(char.IsDigit))
            {
                TempData["Error"] = "El CELULAR es demasiado Largo";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (_appDBContext.Clientes.Any(C => C.NumCelular == cliente.NumCelular))
            {
                TempData["Error"] = "El NÚMERO DE CELULAR ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.Correo) || cliente.Correo.Length > 100)
            {
                TempData["Error"] = "El CORREO es demasiado largo";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (_appDBContext.Clientes.Any(C => C.Correo == cliente.Correo))
            {
                TempData["Error"] = "El CORREO ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            _appDBContext.Clientes.Add(cliente);
            _appDBContext.SaveChanges();
            TempData["Success"] = "Cliente registrado correctamente.";
            return RedirectToAction(nameof(Stakeholders_Cliente));

        }

        // Editar
        [HttpGet]
        public IActionResult EditarCliente(int id)
        {
            var cliente = _appDBContext.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult EditarCliente(Cliente cliente)
        {

            if (string.IsNullOrWhiteSpace(cliente.NomCliente) || cliente.NomCliente.Length > 100 || int.TryParse(cliente.NomCliente, out _))
            {
                TempData["Error"] = "El Nombre es Inválido, introduce un Nombre válido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.TipoCliente) || cliente.TipoCliente.Length > 16)
            {
                TempData["Error"] = "El TIPO DE CLIENTE es Inválido, selecciona un TIPO DE CLIENTE válido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.TipoDocumento) || cliente.TipoDocumento.Length > 22)
            {
                TempData["Error"] = "El TIPO DE DOCUMENTO es Inválido, selecciona un TIPO DE DOCUMENTO válido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.NumDocumento) || cliente.NumDocumento.Length > 13)
            {
                TempData["Error"] = "El DOCUMENTO es invalido";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }
            else if (_appDBContext.Clientes.Any(C => C.NumDocumento == cliente.NumDocumento && C.IdCliente != cliente.IdCliente))
            {
                TempData["Error"] = "El NÚMERO DE DOCUMENTO ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }
            else if (cliente.TipoDocumento == "DNI" && cliente.NumDocumento.Length != 8)
            {
                TempData["Error"] = "El DNI debe tener 8 digitos";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (cliente.TipoDocumento == "RUC" && cliente.NumDocumento.Length != 11)
            {
                TempData["Error"] = "El RUC debe tener 12 digitos";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (cliente.TipoDocumento == "CARNET DE EXTRANJERIA" && cliente.NumDocumento.Length != 12)
            {
                TempData["Error"] = "El CARNET DE EXTRANJERIA debe tener 12 digitos";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.NumCelular) || cliente.NumCelular.Length > 20 || !cliente.NumCelular.All(char.IsDigit))
            {
                TempData["Error"] = "El CELULAR es demasiado Largo";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }
            else if (_appDBContext.Clientes.Any(C => C.NumCelular == cliente.NumCelular && C.IdCliente != cliente.IdCliente))
            {
                TempData["Error"] = "El NÚMERO DE CELULAR ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            else if (string.IsNullOrWhiteSpace(cliente.Correo) || cliente.Correo.Length > 100)
            {
                TempData["Error"] = "El CORREO es demasiado largo";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }
            else if (_appDBContext.Clientes.Any(C => C.Correo == cliente.Correo && C.IdCliente != cliente.IdCliente))
            {
                TempData["Error"] = "El CORREO ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Cliente));
            }

            _appDBContext.Clientes.Update(cliente);
            _appDBContext.SaveChanges();
            TempData["Success"] = "Cliente editado correctamente.";
            return RedirectToAction(nameof(Stakeholders_Cliente));

        }

        // Eliminar
        [HttpGet]
        public IActionResult EliminarCliente(int id)
        {
            var cliente = _appDBContext.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult ConfirmarEliminarCliente(Cliente cliente)
        {
            var entidad = _appDBContext.Clientes.Find(cliente.IdCliente);
            if (entidad != null)
            {
                _appDBContext.Clientes.Remove(entidad);
                _appDBContext.SaveChanges();
            }
            return RedirectToAction(nameof(Stakeholders_Cliente));
        }

        [HttpGet]
        public IActionResult Stakeholders_Proovedor()
        {
            var lista = _appDBContext.Provedor?.ToList();
            return View(lista);
        }

        //Crear
        [HttpGet]
        public IActionResult CrearProovedor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearProovedor(Proovedor proveedor)
        {
            // Remover las alertas previas
            TempData.Remove("Error");
            TempData.Remove("Success");

            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial) || proveedor.RazonSocial.Length > 100 || int.TryParse(proveedor.RazonSocial, out _))
            {
                TempData["Error"] = "La Razón Social es Inválida, introduce una Razón Social válida";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (string.IsNullOrWhiteSpace(proveedor.RUC) || proveedor.RUC.Length != 11 || !proveedor.RUC.All(char.IsDigit))
            {
                TempData["Error"] = "El RUC es Inválido, debe tener 11 dígitos";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (_appDBContext.Provedor.Any(p => p.RUC == proveedor.RUC))
            {
                TempData["Error"] = "El RUC ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.Direccion) && proveedor.Direccion.Length > 150)
            {
                TempData["Error"] = "La DIRECCIÓN es demasiado larga";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.NumCelular) && (proveedor.NumCelular.Length > 15 || !proveedor.NumCelular.All(char.IsDigit)))
            {
                TempData["Error"] = "El CELULAR es inválido";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.NumCelular) && _appDBContext.Provedor.Any(p => p.NumCelular == proveedor.NumCelular))
            {
                TempData["Error"] = "El NÚMERO DE CELULAR ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.Correo) && proveedor.Correo.Length > 100)
            {
                TempData["Error"] = "El CORREO es demasiado largo";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.Correo) && _appDBContext.Provedor.Any(p => p.Correo == proveedor.Correo))
            {
                TempData["Error"] = "El CORREO ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            _appDBContext.Provedor.Add(proveedor);
            _appDBContext.SaveChanges();
            TempData["Success"] = "Proveedor registrado correctamente.";
            return RedirectToAction(nameof(Stakeholders_Proovedor));
        }

        //Editar
        [HttpGet]
        public IActionResult EditarProovedor(int id)
        {
            var proveedor = _appDBContext.Provedor.Find(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        [HttpPost]
        public IActionResult EditarProovedor(Proovedor proveedor)
        {
            // Remover alertas previas
            TempData.Remove("Error");
            TempData.Remove("Success");

            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial) || proveedor.RazonSocial.Length > 100 || int.TryParse(proveedor.RazonSocial, out _))
            {
                TempData["Error"] = "La Razón Social es Inválida, introduce una Razón Social válida";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (string.IsNullOrWhiteSpace(proveedor.RUC) ||
                     proveedor.RUC.Length != 11 ||
                     !proveedor.RUC.All(char.IsDigit))
            {
                TempData["Error"] = "El RUC es Inválido, debe tener 11 dígitos";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (_appDBContext.Provedor.Any(p => p.RUC == proveedor.RUC && p.IdProovedor != proveedor.IdProovedor))
            {
                TempData["Error"] = "El RUC ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.Direccion) && proveedor.Direccion.Length > 150)
            {
                TempData["Error"] = "La DIRECCIÓN es demasiado larga";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.NumCelular) && (proveedor.NumCelular.Length > 15 || !proveedor.NumCelular.All(char.IsDigit)))
            {
                TempData["Error"] = "El CELULAR es inválido";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.NumCelular) && _appDBContext.Provedor.Any(p => p.NumCelular == proveedor.NumCelular && p.IdProovedor != proveedor.IdProovedor))
            {
                TempData["Error"] = "El NÚMERO DE CELULAR ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.Correo) &&
                     proveedor.Correo.Length > 100)
            {
                TempData["Error"] = "El CORREO es demasiado largo";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            else if (!string.IsNullOrWhiteSpace(proveedor.Correo) &&
                     _appDBContext.Provedor.Any(p =>
                     p.Correo == proveedor.Correo &&
                     p.IdProovedor != proveedor.IdProovedor))
            {
                TempData["Error"] = "El CORREO ingresado ya está registrado.";
                return RedirectToAction(nameof(Stakeholders_Proovedor));
            }

            _appDBContext.Provedor.Update(proveedor);
            _appDBContext.SaveChanges();
            TempData["Success"] = "Proveedor actualizado correctamente";
            return RedirectToAction(nameof(Stakeholders_Proovedor));
        }

        //Eliminar
        [HttpGet]
        public IActionResult EliminarProovedor(int id)
        {
            var proveedor = _appDBContext.Provedor.Find(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        [HttpPost]
        public IActionResult ConfirmarEliminarProovedor(Proovedor proveedor)
        {
            var entidad = _appDBContext.Provedor.Find(proveedor.IdProovedor);
            if (entidad != null)
            {
                _appDBContext.Provedor.Remove(entidad);
                _appDBContext.SaveChanges();
            }
            return RedirectToAction(nameof(Stakeholders_Proovedor));
        }

        [HttpGet]
        public IActionResult Stakeholders_Empleado()
        {
            ViewBag.Roles = _appDBContext.Roles.ToList();
            var lista = _appDBContext.empleados?
                .Include(e => e.usuario)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Rol)
                .ToList();

            return View(lista);
        }

        [HttpGet]
        public IActionResult CrearEmpleado()
        {
            ViewBag.Roles = _appDBContext.Roles.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearEmpleado(EmpleadoRegistroVM model)
        {
            ViewBag.Roles = _appDBContext.Roles.ToList();

            // Validaciones de formato
            if (string.IsNullOrWhiteSpace(model.NomEmpleado) || model.NomEmpleado.Length > 100 || int.TryParse(model.NomEmpleado, out _))
                ModelState.AddModelError("NomEmpleado", "El nombre no puede contener números ni estar vacío.");

            if (string.IsNullOrWhiteSpace(model.ApeEmpleado) || model.ApeEmpleado.Length > 100 || int.TryParse(model.ApeEmpleado, out _))
                ModelState.AddModelError("ApeEmpleado", "El apellido no puede contener números ni estar vacío.");

            if (string.IsNullOrWhiteSpace(model.DNI) || model.DNI.Length != 8 || !model.DNI.All(char.IsDigit))
                ModelState.AddModelError("DNI", "El DNI debe tener 8 dígitos.");

            if (string.IsNullOrWhiteSpace(model.Gmail) || model.Gmail.Length > 100)
                ModelState.AddModelError("Gmail", "El correo debe tener el formato 'correo@correo.com'.");

            if (string.IsNullOrWhiteSpace(model.NumCelular) || model.NumCelular.Length != 9 || !model.NumCelular.StartsWith("9") || !model.NumCelular.All(char.IsDigit))
                ModelState.AddModelError("NumCelular", "El número debe tener 9 dígitos y comenzar con 9.");

            if (string.IsNullOrWhiteSpace(model.Usuario) || model.Usuario.Length > 100)
                ModelState.AddModelError("Usuario", "El nombre de usuario no puede contener caracteres especiales.");

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length > 100)
                ModelState.AddModelError("Password", "La contraseña no puede tener más de 100 caracteres.");

            if (model.IdRol == 0 || !_appDBContext.Roles.Any(r => r.IdRol == model.IdRol))
                ModelState.AddModelError("IdRol", "Selecciona un rol válido.");

            // Si hay errores de formato, no consultamos la base de datos aún
            if (!ModelState.IsValid)
                return View(model);

            // Validaciones de existencia en base de datos
            if (_appDBContext.empleados.Any(e => e.dni == model.DNI))
                ModelState.AddModelError("DNI", "Ya existe un empleado con este DNI.");

            if (_appDBContext.empleados.Any(e => e.gmail == model.Gmail))
                ModelState.AddModelError("Gmail", "Este correo ya está registrado.");

            if (_appDBContext.empleados.Any(e => e.numCelular == model.NumCelular))
                ModelState.AddModelError("NumCelular", "Este número de celular ya está registrado.");

            if (_appDBContext.usuarios.Any(u => u.usuario == model.Usuario))
                ModelState.AddModelError("Usuario", "Este usuario ya está registrado.");

            if (!ModelState.IsValid)
                return View(model);

            // Registro
            var empleado = new Empleado
            {
                nomEmpleado = model.NomEmpleado,
                apeEmpleado = model.ApeEmpleado,
                dni = model.DNI,
                gmail = model.Gmail,
                numCelular = model.NumCelular
            };
            _appDBContext.empleados.Add(empleado);
            _appDBContext.SaveChanges();

            var usuario = new Usuario
            {
                usuario = model.Usuario,
                password = new PasswordHasher<Usuario>().HashPassword(null, model.Password),
                IdEmpleado = empleado.IdEmpleado
            };
            _appDBContext.usuarios.Add(usuario);
            _appDBContext.SaveChanges();

            _appDBContext.UserRoles.Add(new User_Rol
            {
                IdUsuario = usuario.IdUsuario,
                IdRol = model.IdRol
            });
            _appDBContext.SaveChanges();

            TempData["Success"] = "✅ Empleado registrado correctamente.";
            return RedirectToAction("Stakeholders_Empleado");
        }


        //Editar
        [HttpGet]
        public IActionResult EditarEmpleado(int id)
        {
            var empleado = _appDBContext.empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [HttpPost]
        public IActionResult EditarEmpleado(Empleado empleado)
        {

            if (string.IsNullOrWhiteSpace(empleado.nomEmpleado) || empleado.nomEmpleado.Length > 100 || int.TryParse(empleado.nomEmpleado, out _))
            {
                TempData["Error"] = "El Nombre Ingresado es inválido.";
                return RedirectToAction(nameof(Stakeholders_Empleado));
            }
            else if (string.IsNullOrWhiteSpace(empleado.apeEmpleado) || empleado.apeEmpleado.Length > 100 || int.TryParse(empleado.apeEmpleado, out _))
            {
                TempData["Error"] = "El Apellido Ingresado es inválido.";
                return RedirectToAction(nameof(Stakeholders_Empleado));
            }
            else if (string.IsNullOrWhiteSpace(empleado.dni) || empleado.dni.Length != 8 || !empleado.dni.All(char.IsDigit))
            {
                TempData["Error"] = "El DNI Ingresado tiene que ser de 8 digitos.";
                return RedirectToAction(nameof(Stakeholders_Empleado));
            }
            else if (_appDBContext.empleados.Any(E => E.dni == empleado.dni && E.IdEmpleado != empleado.IdEmpleado))
            {
                TempData["Error"] = "El DNI ingresado ya está registrado.";
                return RedirectToAction("Stakeholders_Empleado");
            }
            else if (string.IsNullOrWhiteSpace(empleado.gmail) || empleado.gmail.Length > 100)
            {
                TempData["Error"] = "El Correo Ingresado es demasiado largo.";
                return RedirectToAction(nameof(Stakeholders_Empleado));
            }
            else if (_appDBContext.empleados.Any(E => E.gmail == empleado.gmail && E.IdEmpleado != empleado.IdEmpleado))
            {
                TempData["Error"] = "El CORREO ingresado ya está registrado.";
                return RedirectToAction("Stakeholders_Empleado");
            }
            else if (string.IsNullOrWhiteSpace(empleado.numCelular) || empleado.numCelular.Length > 10 || !empleado.numCelular.All(char.IsDigit))
            {
                TempData["Error"] = "El Número Ingresado es demasiado largo.";
                return RedirectToAction(nameof(Stakeholders_Empleado));
            }
            else if (_appDBContext.empleados.Any(E => E.numCelular == empleado.numCelular && E.IdEmpleado != empleado.IdEmpleado))
            {
                TempData["Error"] = "El NÚMERO ingresado ya está registrado.";
                return RedirectToAction("Stakeholders_Empleado");
            }
            _appDBContext.empleados.Update(empleado);
            _appDBContext.SaveChanges();
            TempData["Success"] = "Empleado Editado correctamente.";
            return RedirectToAction(nameof(Stakeholders_Empleado));

        }

        //Eliminar
        [HttpGet]
        public IActionResult EliminarEmpleado(int id)
        {
            var empleado = _appDBContext.empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [HttpPost]
        public IActionResult ConfirmarEliminarEmpleado(Empleado empleado)
        {
            var entidad = _appDBContext.empleados.Find(empleado.IdEmpleado);
            if (entidad != null)
            {
                _appDBContext.empleados.Remove(entidad);
                _appDBContext.SaveChanges();
            }
            return RedirectToAction(nameof(Stakeholders_Empleado));
        }

        [HttpPost]
        public IActionResult GoToMant(ClienteTrabajadorVM cliente_trabajador)
        {
            /*using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Sample Sheet");
            worksheet.Cell("A1").Value = "Hello World 2!";
            worksheet.Cell("A2").FormulaA1 = "MID(A1, 7, 5)";
            workbook.Save();*/

            return View("Mantenimiento", cliente_trabajador);
        }

        [HttpGet]
        public FileResult DescargarExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("EMSI");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Datos");

                worksheet.Row(1).Style.Fill.SetBackground(System.Drawing.Color.AliceBlue);
                worksheet.Cells[1, 1, 1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black, true);
                worksheet.Row(1).CustomHeight = true;
                worksheet.Row(1).Height = 50;

                // Agrega datos
                worksheet.Cells[1, 1].Value = "Nombre";
                worksheet.Cells[1, 2].Value = "Edad";

                worksheet.Cells[2, 1].Value = "Julissa";
                worksheet.Cells[2, 2].Value = 25;

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "reporte.xlsx");
            }
        }

        /*
         //Este funciona pero descarga solo a nivel de backend, es decir, no lo descarga en la computadora del frontend, se guarda el excel dentro de la solución
        [HttpGet]
        public void DownloadFile()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Sample Sheet");
            worksheet.Cell("A1").Value = "Hello World!";
            worksheet.Cell("A2").FormulaA1 = "MID(A1, 7, 5)";
            workbook.SaveAs("HelloWorld.xlsx");
            
        }*/


        /*
        [HttpPost]
        public IActionResult UploadNewProd()//falta VM
        {
            //return View("Mantenimiento");
        }*/
    }
}
