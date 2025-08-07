using EMSI_Corporation.Data;
using EMSI_Corporation.Models;
using EMSI_Corporation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EMSI_Corporation.Controllers
{
    public class MantenimientoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public static List<Mantenimiento> mantenimientos_ = new List<Mantenimiento>();

        public MantenimientoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Index()
        {
            var Mantenimientos = _appDBContext.Mantenimientos.ToList();
            for (int i = 0; i < Mantenimientos.Count; i++)
            {
                Mantenimientos[i].Empleado = _appDBContext.empleados.Find(Mantenimientos[i].Empleado_ID);
                Mantenimientos[i].Extintor = _appDBContext.Extintores.Find(Mantenimientos[i].Extintor_ID);
                Mantenimientos[i].ReporteServicio = _appDBContext.ReportesServicio.Find(Mantenimientos[i].ReporteServicio_ID);
                Mantenimientos[i].ReporteServicio.Cliente = _appDBContext.Clientes.Find(Mantenimientos[i].ReporteServicio.Cliente_ID);
                Mantenimientos[i].ReporteServicio.ComprobanteServicio = _appDBContext.ComprobantesServicio.Find(Mantenimientos[i].ReporteServicio.Comprobante_ID);
            }
            return View(Mantenimientos);
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
            ViewBag.mantenimientos = mantenimientos_;
            ctVM.empleado = _appDBContext.empleados.Find(ctVM.empleadoID);
            ctVM.cliente = _appDBContext.Clientes.Find(ctVM.clienteID);
            ViewBag.cliente_trabajador = ctVM;
            ViewBag.extintores = _appDBContext.Extintores.ToList();

            return View("Registrar");
            

        }

        [HttpPost]
        public IActionResult Registrar(ServicioMantenimientoVM mantVM)
        {
            Cliente cli = _appDBContext.Clientes.Find(mantVM.Cliente_ID);

            ComprobanteServicio cs = new ComprobanteServicio
            {
                Cantidad = mantenimientos_.Count,
                PrecioUnitario = mantVM.PrecioUnitario,
                SubTotal = mantenimientos_.Count * mantVM.PrecioUnitario,
                TipoServicio = "Mantenimiento",
            };
            _appDBContext.ComprobantesServicio.Add(cs);
            List<Mantenimiento> mants = new List<Mantenimiento>();
            
            _appDBContext.SaveChanges();
            ReporteServicio rs = new ReporteServicio
            {
                Cliente = cli,
                Cliente_ID = cli.IdCliente,
                ComprobanteServicio = cs,
                Comprobante_ID = cs.IdComprobante,
                Mantenimientos = mants,
                FirmaCliente = Array.Empty<byte>(),
                ImgEvidencia = Array.Empty<byte>(),
                Observaciones = mantVM.Observaciones
            };
            var rep_ser = _appDBContext.ReportesServicio.Add(rs);
            _appDBContext.SaveChanges();

            foreach (var mant in mantenimientos_)
            {
                mant.ReporteServicio_ID = rep_ser.Entity.IdReporte;
                var m = _appDBContext.Mantenimientos.Add(mant);
                _appDBContext.SaveChanges();
                mants.Add(m.Entity);
            }
            mantenimientos_.Clear();

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
        public IActionResult RegistrarEstadoExtintor(MantenimientoVM mantVM)
        {
            Empleado em = _appDBContext.empleados.Find(mantVM.Empleado_ID);
            Extintor ex = _appDBContext.Extintores.Find(mantVM.Extintor_ID);
            Mantenimiento mant = new Mantenimiento
            {
                AperturaCorrecta = mantVM.AperturaCorrecta,
                BarometroCorrecto = mantVM.BarometroCorrecto,
                BoquillaCorrecta = mantVM.BoquillaCorrecta,
                EstadoIndicador = mantVM.EstadoIndicador,
                EstadoPrecinto = mantVM.EstadoPrecinto,
                ExteriorCorrecto = mantVM.ExteriorCorrecto,
                FechaMantenimiento = DateTime.Now,
                InstruccionesVisibles = mantVM.InstruccionesVisibles,
                LugarAdecuado = mantVM.LugarAdecuado,
                MangueraCorrecta = mantVM.MangueraCorrecta,
                PesoCorrecto = mantVM.PesoCorrecto,
                PresionCorrecta = mantVM.PresionCorrecta,
                Señalización = mantVM.Señalización,
                Usado = mantVM.Usado,
                Visible = mantVM.Visible,
                Empleado_ID = mantVM.Empleado_ID,
                Extintor_ID = mantVM.Extintor_ID,
            };
            mantenimientos_.Add(mant);
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
            ViewBag.mantenimientos = mantenimientos_;
            ViewBag.cliente_trabajador = ct;
            return View("Registrar");
        }
        [HttpGet]
        public IActionResult Borrar(int i)
        {
            Mantenimiento borrado = mantenimientos_[i];
            List<Mantenimiento> lista = new List<Mantenimiento>();
            for (int j = 0; j < mantenimientos_.Count; j++)
            {
                if (j == i) continue;
                lista.Add(mantenimientos_[j]);
            }
            mantenimientos_ = lista;
            ViewBag.extintores = _appDBContext.Extintores.ToList();
            borrado.Extintor = _appDBContext.Extintores.Find(borrado.Extintor_ID);
            Venta ven = _appDBContext.Ventas.Find(borrado.Extintor.Venta_ID);
            ClienteTrabajadorVM ct = new ClienteTrabajadorVM
            {
                cliente = _appDBContext.Clientes.Find(ven.Cliente_ID),
                clienteID = ven.Cliente_ID,
                empleado = _appDBContext.empleados.Find(ven.Empleado_ID),
                empleadoID = ven.Empleado_ID
            };
            ViewBag.mantenimientos = mantenimientos_;
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



                for (int i = 0; i < mantenimientos_.Count; i++)
                {
                    mantenimientos_[i].Empleado = _appDBContext.empleados.Find(mantenimientos_[i].Empleado_ID);
                    mantenimientos_[i].Extintor = _appDBContext.Extintores.Find(mantenimientos_[i].Extintor_ID);
                }

                // Agrega datos
                worksheet.Cells[1, 1].Value = "Id mantenimiento";
                worksheet.Cells[1, 2].Value = "Nombre empleado";
                worksheet.Cells[1, 3].Value = "Id Extintor";    
                worksheet.Cells[1, 4].Value = "Fecha de mantenimiento";
                worksheet.Cells[1, 5].Value = "Usado";
                worksheet.Cells[1, 6].Value = "Visible";
                worksheet.Cells[1, 7].Value = "Instrucciones visibles";
                worksheet.Cells[1, 8].Value = "Apertura correcta";
                worksheet.Cells[1, 9].Value = "Barómetro correcto";
                worksheet.Cells[1, 10].Value = "Boquilla correcta";
                worksheet.Cells[1, 11].Value = "Estado Indicador";
                worksheet.Cells[1, 12].Value = "Estado precinto";
                worksheet.Cells[1, 13].Value = "Exterior correcto";
                worksheet.Cells[1, 14].Value = "Lugar adecuado";
                worksheet.Cells[1, 15].Value = "Manguera correcta";
                worksheet.Cells[1, 16].Value = "Peso correcto";
                worksheet.Cells[1, 17].Value = "Presión correcta";
                worksheet.Cells[1, 18].Value = "Señalización";

                for (int i = 0; i < mantenimientos_.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = mantenimientos_[i].IdMantenimiento;
                    worksheet.Cells[i + 2, 2].Value = mantenimientos_[i].Empleado.nomEmpleado;
                    worksheet.Cells[i + 2, 3].Value = mantenimientos_[i].Extintor_ID;
                    worksheet.Cells[i + 2, 4].Value = mantenimientos_[i].FechaMantenimiento;
                    worksheet.Cells[i + 2, 5].Value = mantenimientos_[i].Usado?"Si":"No";
                    worksheet.Cells[i + 2, 6].Value = mantenimientos_[i].Visible?"Si":"No";
                    worksheet.Cells[i + 2, 7].Value = mantenimientos_[i].InstruccionesVisibles?"Si":"No";
                    worksheet.Cells[i + 2, 8].Value = mantenimientos_[i].AperturaCorrecta?"Si":"No";
                    worksheet.Cells[i + 2, 9].Value = mantenimientos_[i].BarometroCorrecto?"Si":"No";
                    worksheet.Cells[i + 2, 10].Value = mantenimientos_[i].BoquillaCorrecta?"Si":"No";
                    worksheet.Cells[i + 2, 11].Value = mantenimientos_[i].EstadoIndicador?"Correcto":"Incorrecto";
                    worksheet.Cells[i + 2, 12].Value = mantenimientos_[i].EstadoPrecinto?"Correcto":"Incorrecto";
                    worksheet.Cells[i + 2, 13].Value = mantenimientos_[i].ExteriorCorrecto?"Si":"No";
                    worksheet.Cells[i + 2, 14].Value = mantenimientos_[i].LugarAdecuado?"Si":"No";
                    worksheet.Cells[i + 2, 15].Value = mantenimientos_[i].MangueraCorrecta?"Si":"No";
                    worksheet.Cells[i + 2, 16].Value = mantenimientos_[i].PesoCorrecto?"Si":"No";
                    worksheet.Cells[i + 2, 17].Value = mantenimientos_[i].PresionCorrecta?"Si":"No";
                    worksheet.Cells[i + 2, 18].Value = mantenimientos_[i].Señalización?"Correcta":"Incorrecta";
                }

                worksheet.Cells[1, 1, 1, 18].Style.Fill.SetBackground(OfficeOpenXml.Style.ExcelIndexedColor.Indexed30);
                worksheet.Cells[2, 1, mantenimientos_.Count + 2, 18].Style.Fill.SetBackground(OfficeOpenXml.Style.ExcelIndexedColor.Indexed26);
                worksheet.Cells[1, 1, mantenimientos_.Count + 2, 18].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, mantenimientos_.Count + 2, 18].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, mantenimientos_.Count + 2, 18].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, mantenimientos_.Count + 2, 18].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "reporte.xlsx");
            }
        }
    }


}
