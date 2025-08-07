using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMSI_Corporation.Migrations
{
    /// <inheritdoc />
    public partial class Enzom1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_Mantenimiento",
                table: "Mantenimientos");

            migrationBuilder.RenameColumn(
                name: "Mantenimiento",
                table: "Mantenimientos",
                newName: "ReporteServicioIdReporte");

            migrationBuilder.RenameIndex(
                name: "IX_Mantenimientos_Mantenimiento",
                table: "Mantenimientos",
                newName: "IX_Mantenimientos_ReporteServicioIdReporte");

            migrationBuilder.AddForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_ReporteServicioIdReporte",
                table: "Mantenimientos",
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
        }
    }
}
