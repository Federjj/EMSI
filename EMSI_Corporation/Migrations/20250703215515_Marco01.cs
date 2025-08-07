using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMSI_Corporation.Migrations
{
    /// <inheritdoc />
    public partial class Marco01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadDisponible",
                table: "Extintores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadDisponible",
                table: "Extintores");
        }
    }
}
