using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baykus.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CrearGestionMenusPerfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenusSistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Icono = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Page = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    EsVisible = table.Column<bool>(type: "bit", nullable: false),
                    MenuPadreId = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenusSistema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenusSistema_MenusSistema_MenuPadreId",
                        column: x => x.MenuPadreId,
                        principalTable: "MenusSistema",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Perfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    EsAdministrador = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoPerfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoPerfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadoPerfiles_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadoPerfiles_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilMenuPermisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    MenuSistemaId = table.Column<int>(type: "int", nullable: false),
                    PuedeVer = table.Column<bool>(type: "bit", nullable: false),
                    PuedeCrear = table.Column<bool>(type: "bit", nullable: false),
                    PuedeEditar = table.Column<bool>(type: "bit", nullable: false),
                    PuedeEliminar = table.Column<bool>(type: "bit", nullable: false),
                    PuedeAprobar = table.Column<bool>(type: "bit", nullable: false),
                    PuedeRevisar = table.Column<bool>(type: "bit", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilMenuPermisos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilMenuPermisos_MenusSistema_MenuSistemaId",
                        column: x => x.MenuSistemaId,
                        principalTable: "MenusSistema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilMenuPermisos_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoPerfiles_EmpleadoId",
                table: "EmpleadoPerfiles",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoPerfiles_PerfilId",
                table: "EmpleadoPerfiles",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_MenusSistema_MenuPadreId",
                table: "MenusSistema",
                column: "MenuPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilMenuPermisos_MenuSistemaId",
                table: "PerfilMenuPermisos",
                column: "MenuSistemaId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilMenuPermisos_PerfilId",
                table: "PerfilMenuPermisos",
                column: "PerfilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpleadoPerfiles");

            migrationBuilder.DropTable(
                name: "PerfilMenuPermisos");

            migrationBuilder.DropTable(
                name: "MenusSistema");

            migrationBuilder.DropTable(
                name: "Perfiles");
        }
    }
}
