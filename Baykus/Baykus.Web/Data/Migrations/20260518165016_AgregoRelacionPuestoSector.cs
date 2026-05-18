using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baykus.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregoRelacionPuestoSector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "Puestos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Puestos_SectorId",
                table: "Puestos",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Puestos_Sector_SectorId",
                table: "Puestos",
                column: "SectorId",
                principalTable: "Sector",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Puestos_Sector_SectorId",
                table: "Puestos");

            migrationBuilder.DropIndex(
                name: "IX_Puestos_SectorId",
                table: "Puestos");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "Puestos");
        }
    }
}
