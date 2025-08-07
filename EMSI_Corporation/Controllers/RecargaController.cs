using EMSI_Corporation.Data;
using EMSI_Corporation.Models;
using EMSI_Corporation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace EMSI_Corporation.Controllers
{
    public class RecargaController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public static List<Recarga> recargas_ = new List<Recarga>();

        public RecargaController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Index()
        {
            var Recargas = _appDBContext.Recargas.ToList();
            for (int i = 0; i < Recargas.Count; i++)
            {
                Recargas[i].Empleado = _appDBContext.empleados.Find(Recargas[i].Empleado_ID);
                Recargas[i].Extintor = _appDBContext.Extintores.Find(Recargas[i].Extintor_ID);
                Recargas[i].ReporteServicio = _appDBContext.ReportesServicio.Find(Recargas[i].ReporteServicio_ID);
                Recargas[i].ReporteServicio.Cliente = _appDBContext.Clientes.Find(Recargas[i].ReporteServicio.Cliente_ID);
                Recargas[i].ReporteServicio.ComprobanteServicio = _appDBContext.ComprobantesServicio.Find(Recargas[i].ReporteServicio.Comprobante_ID);
            }
            return View(Recargas);
        }
        public IActionResult Registrar(ClienteTrabajadorVM ctVM)//si es sidebar no pasara id de extintor
        {
            /*
            Empleado em = _appDBContext.empleados.Find(v.Empleado_ID);
            Cliente cli = _appDBContext.Clientes.Find(v.Cliente_ID);

            ClienteTrabajadorVM ct = new ClienteTrabajadorVM
            {
                cliente = cli,
                empleado = em
            };*/
            ViewBag.recargas = recargas_;
            ctVM.empleado = _appDBContext.empleados.Find(ctVM.empleadoID);
            ctVM.cliente = _appDBContext.Clientes.Find(ctVM.clienteID);
            ViewBag.cliente_trabajador = ctVM;
            ViewBag.extintores = _appDBContext.Extintores.ToList();

            return View("Registrar");


        }

        [HttpPost]
        public IActionResult Registrar(ServicioRecargaVM recVM)
        {
            Cliente cli = _appDBContext.Clientes.Find(recVM.Cliente_ID);

            ComprobanteServicio cs = new ComprobanteServicio
            {
                Cantidad = recargas_.Count,
                PrecioUnitario = recVM.PrecioUnitario,
                SubTotal = recargas_.Count * recVM.PrecioUnitario,
                TipoServicio = "Recarga",
            };
            _appDBContext.ComprobantesServicio.Add(cs);
            List<Recarga> recs = new List<Recarga>();

            _appDBContext.SaveChanges();
            ReporteServicio rs = new ReporteServicio
            {
                Cliente = cli,
                Cliente_ID = cli.IdCliente,
                ComprobanteServicio = cs,
                Comprobante_ID = cs.IdComprobante,
                Recargas = recs,
                FirmaCliente = Array.Empty<byte>(),
                ImgEvidencia = Array.Empty<byte>(),
                Observaciones = recVM.Observaciones
            };
            var rep_ser = _appDBContext.ReportesServicio.Add(rs);
            _appDBContext.SaveChanges();

            foreach (var rec in recargas_)
            {
                rec.ReporteServicio_ID = rep_ser.Entity.IdReporte;
                var r = _appDBContext.Recargas.Add(rec);
                _appDBContext.SaveChanges();
                recs.Add(r.Entity);
            }
            recargas_.Clear();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RegistrarEstadoExtintor(int id)
        {
            Extintor ext = _appDBContext.Extintores.Find(id);
            Venta vent = _appDBContext.Ventas.Find(ext.Venta_ID);
            Empleado em = _appDBContext.empleados.Find(vent.Empleado_ID);
            ViewBag.extintor = ext;
            ViewBag.venta = vent;
            ViewBag.empleado = em;
            return View("RegistrarEstadoExtintor");
        }

        [HttpPost]
        public IActionResult RegistrarEstadoExtintor(ServicioRecargaVM recVM)
        {
            Empleado em = _appDBContext.empleados.Find(recVM.Empleado_ID);
            Extintor ex = _appDBContext.Extintores.Find(recVM.Extintor_ID);
            Recarga r = new Recarga
            {
                Empleado_ID = recVM.Empleado_ID,
                Extintor_ID = recVM.Extintor_ID,
                CantidadKG = recVM.CantidadKG,
                FechaRecarga = recVM.FechaRecarga,
                MaterialUsado = recVM.MaterialUsado,
                PresionPostRecarga = recVM.PresionPostRecarga,
                ReporteServicio_ID = recVM.ReporteServicio_ID
            };
            recargas_.Add(r);
            /*
            _appDBContext.Mantenimientos.Add(mant);
            _appDBContext.SaveChanges();*/

            ViewBag.extintores = _appDBContext.Extintores.ToList();
            Venta ven = _appDBContext.Ventas.Find(ex.Venta_ID);
            ClienteTrabajadorVM ct = new ClienteTrabajadorVM
            {
                cliente = _appDBContext.Clientes.Find(ven.Cliente_ID),
                clienteID = ven.Cliente_ID,
                empleado = em,
                empleadoID = em.IdEmpleado
            };
            ViewBag.recargas = recargas_;
            ViewBag.cliente_trabajador = ct;
            return View("Registrar");
        }

        [HttpGet]
        public FileResult DescargarExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("EMSI");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Datos");

                //worksheet.Row(1).Style.Fill.SetBackground(System.Drawing.Color.AliceBlue);
                //worksheet.Cells[1, 1, 1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black, true);
                //worksheet.Row(1).CustomHeight = true;
                //worksheet.Row(1).Height = 50;
                worksheet.Rows.Style.WrapText = true;
                worksheet.Rows.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Rows.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                worksheet.Columns.Style.WrapText = true;
                worksheet.Columns.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Columns.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;



                for (int i = 0; i < recargas_.Count; i++)
                {
                    recargas_[i].Empleado = _appDBContext.empleados.Find(recargas_[i].Empleado_ID);
                    recargas_[i].Extintor = _appDBContext.Extintores.Find(recargas_[i].Extintor_ID);
                }

                // Agrega datos
                worksheet.Cells[1, 1].Value = "Id recarga";
                worksheet.Cells[1, 2].Value = "Nombre empleado";
                worksheet.Cells[1, 3].Value = "Id Extintor";
                worksheet.Cells[1, 4].Value = "Fecha de recarga";
                worksheet.Cells[1, 5].Value = "Material usado";
                worksheet.Cells[1, 6].Value = "cantidad KG";
                worksheet.Cells[1, 7].Value = "Presion post recarga";

                for (int i = 0; i < recargas_.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = recargas_[i].IdRecarga;
                    worksheet.Cells[i + 2, 2].Value = recargas_[i].Empleado.nomEmpleado;
                    worksheet.Cells[i + 2, 3].Value = recargas_[i].Extintor_ID;
                    worksheet.Cells[i + 2, 4].Value = recargas_[i].FechaRecarga;
                    worksheet.Cells[i + 2, 5].Value = recargas_[i].MaterialUsado;
                    worksheet.Cells[i + 2, 6].Value = recargas_[i].CantidadKG;
                    worksheet.Cells[i + 2, 7].Value = recargas_[i].PresionPostRecarga;
                }

                worksheet.Cells[1, 1, 1, 7].Style.Fill.SetBackground(OfficeOpenXml.Style.ExcelIndexedColor.Indexed30);
                worksheet.Cells[2, 1, recargas_.Count + 2, 7].Style.Fill.SetBackground(OfficeOpenXml.Style.ExcelIndexedColor.Indexed26);
                worksheet.Cells[1, 1, recargas_.Count + 2, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, recargas_.Count + 2, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, recargas_.Count + 2, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, recargas_.Count + 2, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "reporte.xlsx");
            }
        }
    }
}
