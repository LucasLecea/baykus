using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baykus.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModuloPlanillasHorarias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cuil",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Empleados",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JornadaLaboralId",
                table: "Empleados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Legajo",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoBaja",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereCargaHoraria",
                table: "Empleados",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Feriados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsNacional = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feriados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JornadasLaborales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HorasDiariasObjetivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasSemanalesObjetivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoraEntradaDefault = table.Column<TimeOnly>(type: "time", nullable: true),
                    HoraSalidaDefault = table.Column<TimeOnly>(type: "time", nullable: true),
                    PermiteHorasExtra = table.Column<bool>(type: "bit", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JornadasLaborales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanillasHorarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    FechaDesde = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaHasta = table.Column<DateOnly>(type: "date", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    TotalHorasNormales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHorasExtra50 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHorasExtra100 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHorasNocturnas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHorasFeriado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHorasTrabajadas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ObservacionEmpleado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObservacionAprobador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaPresentacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaRechazo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCargaId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioAprobacionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpleadoNombreSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpleadoDniSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectorNombreSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PuestoNombreSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JornadaNombreSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanillasHorarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanillasHorarias_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadosHistorialLaboral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: true),
                    PuestoId = table.Column<int>(type: "int", nullable: true),
                    JornadaLaboralId = table.Column<int>(type: "int", nullable: true),
                    FechaDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadosHistorialLaboral", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadosHistorialLaboral_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadosHistorialLaboral_JornadasLaborales_JornadaLaboralId",
                        column: x => x.JornadaLaboralId,
                        principalTable: "JornadasLaborales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmpleadosHistorialLaboral_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmpleadosHistorialLaboral_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlanillasHorariasDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanillaHorariaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraEntrada = table.Column<TimeOnly>(type: "time", nullable: true),
                    HoraSalida = table.Column<TimeOnly>(type: "time", nullable: true),
                    MinutosDescanso = table.Column<int>(type: "int", nullable: false),
                    HorasNormales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasExtra50 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasExtra100 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasNocturnas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasFeriado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHoras = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoDia = table.Column<int>(type: "int", nullable: false),
                    EsFeriado = table.Column<bool>(type: "bit", nullable: false),
                    EsFinDeSemana = table.Column<bool>(type: "bit", nullable: false),
                    TareaRealizada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanillasHorariasDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanillasHorariasDetalles_PlanillasHorarias_PlanillaHorariaId",
                        column: x => x.PlanillaHorariaId,
                        principalTable: "PlanillasHorarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanillasHorariasHistorial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanillaHorariaId = table.Column<int>(type: "int", nullable: false),
                    EstadoAnterior = table.Column<int>(type: "int", nullable: true),
                    EstadoNuevo = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanillasHorariasHistorial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanillasHorariasHistorial_PlanillasHorarias_PlanillaHorariaId",
                        column: x => x.PlanillaHorariaId,
                        principalTable: "PlanillasHorarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_JornadaLaboralId",
                table: "Empleados",
                column: "JornadaLaboralId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosHistorialLaboral_EmpleadoId",
                table: "EmpleadosHistorialLaboral",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosHistorialLaboral_JornadaLaboralId",
                table: "EmpleadosHistorialLaboral",
                column: "JornadaLaboralId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosHistorialLaboral_PuestoId",
                table: "EmpleadosHistorialLaboral",
                column: "PuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosHistorialLaboral_SectorId",
                table: "EmpleadosHistorialLaboral",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanillasHorarias_EmpleadoId",
                table: "PlanillasHorarias",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanillasHorariasDetalles_PlanillaHorariaId",
                table: "PlanillasHorariasDetalles",
                column: "PlanillaHorariaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanillasHorariasHistorial_PlanillaHorariaId",
                table: "PlanillasHorariasHistorial",
                column: "PlanillaHorariaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_JornadasLaborales_JornadaLaboralId",
                table: "Empleados",
                column: "JornadaLaboralId",
                principalTable: "JornadasLaborales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_JornadasLaborales_JornadaLaboralId",
                table: "Empleados");

            migrationBuilder.DropTable(
                name: "EmpleadosHistorialLaboral");

            migrationBuilder.DropTable(
                name: "Feriados");

            migrationBuilder.DropTable(
                name: "PlanillasHorariasDetalles");

            migrationBuilder.DropTable(
                name: "PlanillasHorariasHistorial");

            migrationBuilder.DropTable(
                name: "JornadasLaborales");

            migrationBuilder.DropTable(
                name: "PlanillasHorarias");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_JornadaLaboralId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "Cuil",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "JornadaLaboralId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "Legajo",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "MotivoBaja",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "RequiereCargaHoraria",
                table: "Empleados");
        }
    }
}
