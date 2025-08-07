using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMSI_Corporation.Migrations
{
    /// <inheritdoc />
    public partial class enzo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_Mantenimiento",
                table: "Mantenimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Recargas_ReportesServicio_Recarga",
                table: "Recargas");

            migrationBuilder.RenameColumn(
                name: "Recarga",
                table: "Recargas",
                newName: "ReporteServicioIdReporte");

            migrationBuilder.RenameIndex(
                name: "IX_Recargas_Recarga",
                table: "Recargas",
                newName: "IX_Recargas_ReporteServicioIdReporte");

            migrationBuilder.RenameColumn(
                name: "Mantenimiento",
                table: "Mantenimientos",
                newName: "ReporteServicioIdReporte");

            migrationBuilder.RenameIndex(
                name: "IX_Mantenimientos_Mantenimiento",
                table: "Mantenimientos",
                newName: "IX_Mantenimientos_ReporteServicioIdReporte");

            migrationBuilder.CreateTable(
                name: "EventosCalendario",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosCalendario", x => x.IdEvento);
                    table.ForeignKey(
                        name: "FK_EventosCalendario_Empleado_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleado",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventosCalendario_EmpleadoId",
                table: "EventosCalendario",
                column: "EmpleadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_ReporteServicioIdReporte",
                table: "Mantenimientos",
                column: "ReporteServicioIdReporte",
                principalTable: "ReportesServicio",
                principalColumn: "IdReporte");

            migrationBuilder.AddForeignKey(
                name: "FK_Recargas_ReportesServicio_ReporteServicioIdReporte",
                table: "Recargas",
                column: "ReporteServicioIdReporte",
                principalTable: "ReportesServicio",
                principalColumn: "IdReporte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_ReporteServicioIdReporte",
                table: "Mantenimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Recargas_ReportesServicio_ReporteServicioIdReporte",
                table: "Recargas");

            migrationBuilder.DropTable(
                name: "EventosCalendario");

            migrationBuilder.RenameColumn(
                name: "ReporteServicioIdReporte",
                table: "Recargas",
                newName: "Recarga");

            migrationBuilder.RenameIndex(
                name: "IX_Recargas_ReporteServicioIdReporte",
                table: "Recargas",
                newName: "IX_Recargas_Recarga");

            migrationBuilder.RenameColumn(
                name: "ReporteServicioIdReporte",
                table: "Mantenimientos",
                newName: "Mantenimiento");

            migrationBuilder.RenameIndex(
                name: "IX_Mantenimientos_ReporteServicioIdReporte",
                table: "Mantenimientos",
                newName: "IX_Mantenimientos_Mantenimiento");

            migrationBuilder.AddForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_Mantenimiento",
                table: "Mantenimientos",
                column: "Mantenimiento",
                principalTable: "ReportesServicio",
                principalColumn: "IdReporte");

            migrationBuilder.AddForeignKey(
                name: "FK_Recargas_ReportesServicio_Recarga",
                table: "Recargas",
                column: "Recarga",
                principalTable: "ReportesServicio",
                principalColumn: "IdReporte");
        }
    }
}
