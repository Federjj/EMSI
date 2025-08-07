using EMSI_Corporation.ViewModels;
using EMSI_Corporation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMSI_Corporation.Data;

public class RecepcionController : Controller
{
    private readonly AppDBContext _appDBContext;

    public RecepcionController(AppDBContext appDBContext)
    {
        _appDBContext = appDBContext;
    }

    //Ventana Principal
    [HttpGet]
    public IActionResult Index()
    {
        var proovedores = _appDBContext.Provedor.ToList();
        ViewBag.Proovedores = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(proovedores, "IdProovedor", "RazonSocial");
        return View();
    }

    //Realizar recepción
    [HttpGet]
    public IActionResult CrearRecepcion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CrearRecepcion(DateTime Fecha, int ProovedorId)
    {
        var nuevaRecepcion = new Recepcion
        {
            Fecha = Fecha,
            ProovedorId = ProovedorId
        };

        _appDBContext.Recepcion.Add(nuevaRecepcion);
        await _appDBContext.SaveChangesAsync();

        // Redirigir a la vista donde se registran los extintores, pasando el ID de la nueva recepción
        return RedirectToAction("CrearRecepcionAgrupada", new { recepcionId = nuevaRecepcion.IdRecepcion });
    }

    // GET: Mostrar formulario de recepción agrupada
    [HttpGet]
    public IActionResult CrearRecepcionAgrupada()
    {
        return View();
    }

    // POST: Procesar la recepción agrupada
    [HttpPost]
    public async Task<IActionResult> RecepcionarAgrupado(List<RecepcionVM> listaRecepcion)
    {
        foreach (var item in listaRecepcion)
        {
            var extintorExistente = await _appDBContext.Extintores.FirstOrDefaultAsync(e =>
                e.TipoAgente == item.TipoAgente &&
                e.ClaseFuego == item.ClaseFuego &&
                e.CapacidadKG == item.CapacidadKG &&
                e.Estado == "Operativo"
            );

            if (extintorExistente != null)
            {
                extintorExistente.CantidadDisponible += item.Cantidad;
                _appDBContext.Extintores.Update(extintorExistente);
            }
            else
            {
                var nuevoExtintor = new Extintor
                {
                    TipoAgente = item.TipoAgente,
                    ClaseFuego = item.ClaseFuego,
                    CapacidadKG = item.CapacidadKG,
                    CantidadDisponible = item.Cantidad,
                    FechaVencimiento = item.FechaVencimiento,
                    Estado = "Operativo"
                };

                _appDBContext.Extintores.Add(nuevoExtintor);
            }
        }

        await _appDBContext.SaveChangesAsync();
        return RedirectToAction("Index"); // o donde desees redirigir luego
    }
}
