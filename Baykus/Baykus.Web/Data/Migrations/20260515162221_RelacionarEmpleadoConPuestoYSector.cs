using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baykus.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionarEmpleadoConPuestoYSector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Puesto",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Empleados");

            migrationBuilder.AddColumn<int>(
                name: "PuestoId",
                table: "Empleados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "Empleados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_PuestoId",
                table: "Empleados",
                column: "PuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_SectorId",
                table: "Empleados",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Puestos_PuestoId",
                table: "Empleados",
                column: "PuestoId",
                principalTable: "Puestos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Sector_SectorId",
                table: "Empleados",
                column: "SectorId",
                principalTable: "Sector",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Puestos_PuestoId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Sector_SectorId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_PuestoId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_SectorId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "PuestoId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "Empleados");

            migrationBuilder.AddColumn<string>(
                name: "Puesto",
                table: "Empleados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "Empleados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
