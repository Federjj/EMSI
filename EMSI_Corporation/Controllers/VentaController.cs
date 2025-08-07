using EMSI_Corporation.Data;
using EMSI_Corporation.Models;
using EMSI_Corporation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EMSI_Corporation.Controllers
{

    public class VentaController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public VentaController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Index()
        {
            var ventas = _appDBContext.Ventas.ToList();
            var extintores = _appDBContext.Extintores.ToList();

            for (int i = 0; i < ventas.Count; i++)
            {
                ventas[i].Cliente = _appDBContext.Clientes.Find(ventas[i].Cliente_ID);
                ventas[i].Empleado = _appDBContext.empleados.Find(ventas[i].Empleado_ID);
            }

            ViewBag.extintores = extintores;
            return View(ventas);
        }

        public IActionResult VentasPorId(int id)
        {
            var Extintor = _appDBContext.Extintores.Find(id);
            Venta vent = Extintor.Venta;
            ViewBag.ExtintorId = id;
            return View(vent);
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            ViewBag.clientes = _appDBContext.Clientes.ToList();
            ViewBag.empleados = _appDBContext.empleados.ToList();
            ViewBag.extintores = _appDBContext.Extintores.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(VentaVM ventaVM)
        {
            /*
            int hola = 0;
            if(int.TryParse(ventaVM.MetodoPago, out hola))
            {
                ViewBag.clientes = _appDBContext.Clientes.ToList();
                ViewBag.empleados = _appDBContext.empleados.ToList();
                ViewData["error"] = "Datos incorrectos";
                return View();

            }
            */
            try
            {
                int hola = 0;
                if (int.TryParse(ventaVM.MetodoPago, out hola))
                {
                    throw new Exception();

                }
                Cliente? cli = _appDBContext.Clientes.Find(ventaVM.Cliente_ID);
                Empleado? empleado = _appDBContext.empleados.Find(ventaVM.Empleado_ID);

                Venta venta = new Venta
                {
                    Cliente = cli,
                    Cliente_ID = ventaVM.Cliente_ID,
                    Empleado = empleado,
                    Empleado_ID = ventaVM.Empleado_ID,
                    FechaVenta = ventaVM.FechaVenta,
                    MetodoPago = ventaVM.MetodoPago,
                    Total = ventaVM.Total
                };
                _appDBContext.Ventas.Add(venta);
                _appDBContext.SaveChanges();

                Extintor ext = _appDBContext.Extintores.Find(ventaVM.Extintor_ID);
                ext.Venta_ID = venta.IdVenta;
                _appDBContext.Extintores.Update(ext);
                _appDBContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.clientes = _appDBContext.Clientes.ToList();
                ViewBag.empleados = _appDBContext.empleados.ToList();
                ViewBag.extintores = _appDBContext.Extintores.ToList();
                ViewData["error"] = "Datos incorrectos";
                return View();
            }
        }
    }
}
