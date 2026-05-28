using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baykus.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRelacionEmpleadoApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Empleados",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_ApplicationUserId",
                table: "Empleados",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_AspNetUsers_ApplicationUserId",
                table: "Empleados",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_AspNetUsers_ApplicationUserId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_ApplicationUserId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Empleados");
        }
    }
}
