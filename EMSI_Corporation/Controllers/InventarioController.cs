using EMSI_Corporation.Data;
using EMSI_Corporation.Views.Inventario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EMSI_Corporation.Controllers
{
    public class InventarioController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public InventarioController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Inventario()
        {
            var extintores = await _appDBContext.Extintores.ToListAsync();
            return View(extintores);
        }

        [HttpGet]
        public IActionResult VerPDF()
        {
            try
            {
                var pdf = new InventarioPDF(_appDBContext);
                var documento = pdf.CreatePDF();

                var stream = new MemoryStream();
                documento.GeneratePdf(stream);
                stream.Position = 0;

                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                return Content("⚠️ Error generando PDF: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult DescargarPDF()
        {
            try
            {
                var pdf = new InventarioPDF(_appDBContext);
                var documento = pdf.CreatePDF();

                var stream = new MemoryStream();
                documento.GeneratePdf(stream);
                stream.Position = 0;

                // Forzar la descarga del PDF
                return File(stream, "application/pdf", "InventarioExtintores.pdf");
            }
            catch (Exception ex)
            {
                return Content("⚠️ Error generando PDF: " + ex.Message);
            }
        }
    }
}
