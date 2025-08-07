using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMSI_Corporation.Migrations
{
    /// <inheritdoc />
    public partial class migmant_rec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_ReporteServicioIdReporte",
                table: "Mantenimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportesServicio_Recargas_Recarga_ID",
                table: "ReportesServicio");

            migrationBuilder.DropIndex(
                name: "IX_ReportesServicio_Recarga_ID",
                table: "ReportesServicio");

            migrationBuilder.DropColumn(
                name: "Recarga_ID",
                table: "ReportesServicio");

            migrationBuilder.RenameColumn(
                name: "ReporteServicioIdReporte",
                table: "Mantenimientos",
                newName: "Mantenimiento");

            migrationBuilder.RenameIndex(
                name: "IX_Mantenimientos_ReporteServicioIdReporte",
                table: "Mantenimientos",
                newName: "IX_Mantenimientos_Mantenimiento");

            migrationBuilder.AddColumn<int>(
                name: "Recarga",
                table: "Recargas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReporteServicio_ID",
                table: "Recargas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recargas_Recarga",
                table: "Recargas",
                column: "Recarga");

            migrationBuilder.CreateIndex(
                name: "IX_Recargas_ReporteServicio_ID",
                table: "Recargas",
                column: "ReporteServicio_ID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Recargas_ReportesServicio_ReporteServicio_ID",
                table: "Recargas",
                column: "ReporteServicio_ID",
                principalTable: "ReportesServicio",
                principalColumn: "IdReporte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_Mantenimiento",
                table: "Mantenimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Recargas_ReportesServicio_Recarga",
                table: "Recargas");

            migrationBuilder.DropForeignKey(
                name: "FK_Recargas_ReportesServicio_ReporteServicio_ID",
                table: "Recargas");

            migrationBuilder.DropIndex(
                name: "IX_Recargas_Recarga",
                table: "Recargas");

            migrationBuilder.DropIndex(
                name: "IX_Recargas_ReporteServicio_ID",
                table: "Recargas");

            migrationBuilder.DropColumn(
                name: "Recarga",
                table: "Recargas");

            migrationBuilder.DropColumn(
                name: "ReporteServicio_ID",
                table: "Recargas");

            migrationBuilder.RenameColumn(
                name: "Mantenimiento",
                table: "Mantenimientos",
                newName: "ReporteServicioIdReporte");

            migrationBuilder.RenameIndex(
                name: "IX_Mantenimientos_Mantenimiento",
                table: "Mantenimientos",
                newName: "IX_Mantenimientos_ReporteServicioIdReporte");

            migrationBuilder.AddColumn<int>(
                name: "Recarga_ID",
                table: "ReportesServicio",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportesServicio_Recarga_ID",
                table: "ReportesServicio",
                column: "Recarga_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Mantenimientos_ReportesServicio_ReporteServicioIdReporte",
                table: "Mantenimientos",
                column: "ReporteServicioIdReporte",
                principalTable: "ReportesServicio",
                principalColumn: "IdReporte");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportesServicio_Recargas_Recarga_ID",
                table: "ReportesServicio",
                column: "Recarga_ID",
                principalTable: "Recargas",
                principalColumn: "IdRecarga");
        }
    }
}
