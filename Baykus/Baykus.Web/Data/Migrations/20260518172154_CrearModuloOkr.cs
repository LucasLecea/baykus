using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baykus.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CrearModuloOkr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObjetivosOkr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    PuestoId = table.Column<int>(type: "int", nullable: true),
                    EmpleadoResponsableId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Progreso = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivosOkr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjetivosOkr_Empleados_EmpleadoResponsableId",
                        column: x => x.EmpleadoResponsableId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjetivosOkr_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ObjetivosOkr_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosClaveOkr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjetivoOkrId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Progreso = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosClaveOkr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadosClaveOkr_ObjetivosOkr_ObjetivoOkrId",
                        column: x => x.ObjetivoOkrId,
                        principalTable: "ObjetivosOkr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeguimientosOkr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjetivoOkrId = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientosOkr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeguimientosOkr_ObjetivosOkr_ObjetivoOkrId",
                        column: x => x.ObjetivoOkrId,
                        principalTable: "ObjetivosOkr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivosOkr_EmpleadoResponsableId",
                table: "ObjetivosOkr",
                column: "EmpleadoResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivosOkr_PuestoId",
                table: "ObjetivosOkr",
                column: "PuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivosOkr_SectorId",
                table: "ObjetivosOkr",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosClaveOkr_ObjetivoOkrId",
                table: "ResultadosClaveOkr",
                column: "ObjetivoOkrId");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosOkr_ObjetivoOkrId",
                table: "SeguimientosOkr",
                column: "ObjetivoOkrId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosClaveOkr");

            migrationBuilder.DropTable(
                name: "SeguimientosOkr");

            migrationBuilder.DropTable(
                name: "ObjetivosOkr");
        }
    }
}
