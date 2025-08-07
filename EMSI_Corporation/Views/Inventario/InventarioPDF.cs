using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using EMSI_Corporation.Models;
using EMSI_Corporation.Data;
using System;
using System.Linq;

namespace EMSI_Corporation.Views.Inventario
{
    public class InventarioPDF
    {
        private readonly AppDBContext _context;

        public InventarioPDF(AppDBContext context)
        {
            _context = context;
        }

        public IDocument CreatePDF()
        {
            var extintores = _context.Extintores.ToList();
            string rojoCorporativo = "#B20000";

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    // CABECERA
                    page.Header().Column(header =>
                    {
                        header.Item().AlignCenter().Text("INVENTARIO DE EXTINTORES")
                            .FontSize(18).Bold().FontColor(rojoCorporativo);
                        header.Item().AlignCenter().Text("CORPORACIÓN EMSI")
                            .FontSize(12).FontColor(Colors.Grey.Darken3);
                        header.Item().AlignCenter().Text("Mza. B Lote. 14 Pp.Jj Baños Punta")
                            .FontSize(10);
                        header.Item().AlignCenter().Text("964214945 / 944259981 - corporacion.emsi@gmail.com")
                            .FontSize(10);
                    });

                    // TABLA
                    page.Content().PaddingVertical(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1); // ID
                            columns.RelativeColumn(2); // Tipo Agente
                            columns.RelativeColumn(2); // Clase Fuego
                            columns.RelativeColumn(2); // Capacidad
                            columns.RelativeColumn(2); // Cantidad
                        });

                        // ENCABEZADO
                        table.Header(header =>
                        {
                            header.Cell().Element(CellHeaderStyle).Text("ID").Bold();
                            header.Cell().Element(CellHeaderStyle).Text("Tipo de Agente").Bold();
                            header.Cell().Element(CellHeaderStyle).Text("Clase de Fuego").Bold();
                            header.Cell().Element(CellHeaderStyle).Text("Capacidad (KG)").Bold();
                            header.Cell().Element(CellHeaderStyle).Text("Cantidad").Bold();

                            static IContainer CellHeaderStyle(IContainer container) =>
                                container.DefaultTextStyle(x => x.FontSize(11).Bold().FontColor(Colors.White))
                                         .Background("#B20000")
                                         .AlignCenter()
                                         .PaddingVertical(6)
                                         .BorderBottom(1);
                        });

                        // FILAS DE DATOS
                        foreach (var ext in extintores)
                        {
                            table.Cell().Element(CellBodyStyle).Text(ext.IdExtintor.ToString());
                            table.Cell().Element(CellBodyStyle).Text(ext.TipoAgente);
                            table.Cell().Element(CellBodyStyle).Text(ext.ClaseFuego);
                            table.Cell().Element(CellBodyStyle).Text($"{ext.CapacidadKG} KG");
                            table.Cell().Element(CellBodyStyle).Text(ext.CantidadDisponible.ToString());

                            static IContainer CellBodyStyle(IContainer container) =>
                                container.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken3))
                                         .PaddingVertical(4)
                                         .AlignCenter()
                                         .BorderBottom(0.5f);
                        }
                    });

                    // PIE DE PÁGINA
                    page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(9).Italic().FontColor(Colors.Grey.Darken1);
                });
            });
        }
    }
}
