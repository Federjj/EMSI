using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EMSI_Corporation.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_PostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomCliente = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TipoCliente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TipoDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumDocumento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NumCelular = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesServicio",
                columns: table => new
                {
                    IdComprobante = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoServicio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesServicio", x => x.IdComprobante);
                });

            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    IdEmpleado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomEmpleado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apeEmpleado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    dni = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    gmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    numCelular = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.IdEmpleado);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    IdMenu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomMenu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.IdMenu);
                });

            migrationBuilder.CreateTable(
                name: "Proovedor",
                columns: table => new
                {
                    IdProovedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RazonSocial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RUC = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    NumCelular = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proovedor", x => x.IdProovedor);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomRol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "ReportesServicio",
                columns: table => new
                {
                    IdReporte = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirmaCliente = table.Column<byte[]>(type: "bytea", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    ImgEvidencia = table.Column<byte[]>(type: "bytea", nullable: false),
                    Cliente_ID = table.Column<int>(type: "integer", nullable: false),
                    Comprobante_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportesServicio", x => x.IdReporte);
                    table.ForeignKey(
                        name: "FK_ReportesServicio_Clientes_Cliente_ID",
                        column: x => x.Cliente_ID,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportesServicio_ComprobantesServicio_Comprobante_ID",
                        column: x => x.Comprobante_ID,
                        principalTable: "ComprobantesServicio",
                        principalColumn: "IdComprobante",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventosCalendario",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EmpleadoId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", maxLength: 100, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IdEmpleado = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_Empleado_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaVenta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetodoPago = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Cliente_ID = table.Column<int>(type: "integer", nullable: false),
                    Empleado_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.IdVenta);
                    table.ForeignKey(
                        name: "FK_Ventas_Clientes_Cliente_ID",
                        column: x => x.Cliente_ID,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ventas_Empleado_Empleado_ID",
                        column: x => x.Empleado_ID,
                        principalTable: "Empleado",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitasTecnicas",
                columns: table => new
                {
                    IdVisita = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    Empleado_ID = table.Column<int>(type: "integer", nullable: false),
                    Cliente_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitasTecnicas", x => x.IdVisita);
                    table.ForeignKey(
                        name: "FK_VisitasTecnicas_Clientes_Cliente_ID",
                        column: x => x.Cliente_ID,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitasTecnicas_Empleado_Empleado_ID",
                        column: x => x.Empleado_ID,
                        principalTable: "Empleado",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rol_Menu",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    IdMenu = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol_Menu", x => new { x.IdMenu, x.IdRol });
                    table.ForeignKey(
                        name: "FK_Rol_Menu_Menu_IdMenu",
                        column: x => x.IdMenu,
                        principalTable: "Menu",
                        principalColumn: "IdMenu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rol_Menu_Rol_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recepcion",
                columns: table => new
                {
                    IdRecepcion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true),
                    ProovedorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepcion", x => x.IdRecepcion);
                    table.ForeignKey(
                        name: "FK_Recepcion_Proovedor_ProovedorId",
                        column: x => x.ProovedorId,
                        principalTable: "Proovedor",
                        principalColumn: "IdProovedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recepcion_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Rol",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Rol", x => new { x.IdUsuario, x.IdRol });
                    table.ForeignKey(
                        name: "FK_User_Rol_Rol_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Rol_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comprobantes",
                columns: table => new
                {
                    IdComprobante = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoComprobante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NroComprobante = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MontoTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Venta_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobantes", x => x.IdComprobante);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Ventas_Venta_ID",
                        column: x => x.Venta_ID,
                        principalTable: "Ventas",
                        principalColumn: "IdVenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformesTecnicos",
                columns: table => new
                {
                    IdInforme = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaInforme = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Informe_PDF = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Visita_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformesTecnicos", x => x.IdInforme);
                    table.ForeignKey(
                        name: "FK_InformesTecnicos_VisitasTecnicas_Visita_ID",
                        column: x => x.Visita_ID,
                        principalTable: "VisitasTecnicas",
                        principalColumn: "IdVisita",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Extintores",
                columns: table => new
                {
                    IdExtintor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoAgente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ClaseFuego = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CapacidadKG = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CantidadDisponible = table.Column<int>(type: "integer", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Venta_ID = table.Column<int>(type: "integer", nullable: true),
                    RecepcionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extintores", x => x.IdExtintor);
                    table.ForeignKey(
                        name: "FK_Extintores_Recepcion_RecepcionId",
                        column: x => x.RecepcionId,
                        principalTable: "Recepcion",
                        principalColumn: "IdRecepcion");
                    table.ForeignKey(
                        name: "FK_Extintores_Ventas_Venta_ID",
                        column: x => x.Venta_ID,
                        principalTable: "Ventas",
                        principalColumn: "IdVenta");
                });

            migrationBuilder.CreateTable(
                name: "Mantenimientos",
                columns: table => new
                {
                    IdMantenimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaMantenimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LugarAdecuado = table.Column<bool>(type: "boolean", nullable: false),
                    Señalización = table.Column<bool>(type: "boolean", nullable: false),
                    Visible = table.Column<bool>(type: "boolean", nullable: false),
                    Usado = table.Column<bool>(type: "boolean", nullable: false),
                    EstadoPrecinto = table.Column<bool>(type: "boolean", nullable: false),
                    EstadoIndicador = table.Column<bool>(type: "boolean", nullable: false),
                    PresionCorrecta = table.Column<bool>(type: "boolean", nullable: false),
                    ExteriorCorrecto = table.Column<bool>(type: "boolean", nullable: false),
                    PesoCorrecto = table.Column<bool>(type: "boolean", nullable: false),
                    MangueraCorrecta = table.Column<bool>(type: "boolean", nullable: false),
                    BoquillaCorrecta = table.Column<bool>(type: "boolean", nullable: false),
                    InstruccionesVisibles = table.Column<bool>(type: "boolean", nullable: false),
                    AperturaCorrecta = table.Column<bool>(type: "boolean", nullable: false),
                    BarometroCorrecto = table.Column<bool>(type: "boolean", nullable: false),
                    Empleado_ID = table.Column<int>(type: "integer", nullable: false),
                    Extintor_ID = table.Column<int>(type: "integer", nullable: false),
                    ReporteServicio_ID = table.Column<int>(type: "integer", nullable: false),
                    ReporteServicioIdReporte = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimientos", x => x.IdMantenimiento);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_Empleado_Empleado_ID",
                        column: x => x.Empleado_ID,
                        principalTable: "Empleado",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_Extintores_Extintor_ID",
                        column: x => x.Extintor_ID,
                        principalTable: "Extintores",
                        principalColumn: "IdExtintor");
                    table.ForeignKey(
                        name: "FK_Mantenimientos_ReportesServicio_ReporteServicioIdReporte",
                        column: x => x.ReporteServicioIdReporte,
                        principalTable: "ReportesServicio",
                        principalColumn: "IdReporte");
                    table.ForeignKey(
                        name: "FK_Mantenimientos_ReportesServicio_ReporteServicio_ID",
                        column: x => x.ReporteServicio_ID,
                        principalTable: "ReportesServicio",
                        principalColumn: "IdReporte");
                });

            migrationBuilder.CreateTable(
                name: "Recargas",
                columns: table => new
                {
                    IdRecarga = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaRecarga = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaterialUsado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CantidadKG = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    PresionPostRecarga = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Extintor_ID = table.Column<int>(type: "integer", nullable: false),
                    Empleado_ID = table.Column<int>(type: "integer", nullable: false),
                    ReporteServicio_ID = table.Column<int>(type: "integer", nullable: false),
                    ReporteServicioIdReporte = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recargas", x => x.IdRecarga);
                    table.ForeignKey(
                        name: "FK_Recargas_Empleado_Empleado_ID",
                        column: x => x.Empleado_ID,
                        principalTable: "Empleado",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recargas_Extintores_Extintor_ID",
                        column: x => x.Extintor_ID,
                        principalTable: "Extintores",
                        principalColumn: "IdExtintor");
                    table.ForeignKey(
                        name: "FK_Recargas_ReportesServicio_ReporteServicioIdReporte",
                        column: x => x.ReporteServicioIdReporte,
                        principalTable: "ReportesServicio",
                        principalColumn: "IdReporte");
                    table.ForeignKey(
                        name: "FK_Recargas_ReportesServicio_ReporteServicio_ID",
                        column: x => x.ReporteServicio_ID,
                        principalTable: "ReportesServicio",
                        principalColumn: "IdReporte");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_Venta_ID",
                table: "Comprobantes",
                column: "Venta_ID");

            migrationBuilder.CreateIndex(
                name: "IX_EventosCalendario_EmpleadoId",
                table: "EventosCalendario",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Extintores_RecepcionId",
                table: "Extintores",
                column: "RecepcionId");

            migrationBuilder.CreateIndex(
                name: "IX_Extintores_Venta_ID",
                table: "Extintores",
                column: "Venta_ID");

            migrationBuilder.CreateIndex(
                name: "IX_InformesTecnicos_Visita_ID",
                table: "InformesTecnicos",
                column: "Visita_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_Empleado_ID",
                table: "Mantenimientos",
                column: "Empleado_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_Extintor_ID",
                table: "Mantenimientos",
                column: "Extintor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_ReporteServicio_ID",
                table: "Mantenimientos",
                column: "ReporteServicio_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_ReporteServicioIdReporte",
                table: "Mantenimientos",
                column: "ReporteServicioIdReporte");

            migrationBuilder.CreateIndex(
                name: "IX_Recargas_Empleado_ID",
                table: "Recargas",
                column: "Empleado_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Recargas_Extintor_ID",
                table: "Recargas",
                column: "Extintor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Recargas_ReporteServicio_ID",
                table: "Recargas",
                column: "ReporteServicio_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Recargas_ReporteServicioIdReporte",
                table: "Recargas",
                column: "ReporteServicioIdReporte");

            migrationBuilder.CreateIndex(
                name: "IX_Recepcion_ProovedorId",
                table: "Recepcion",
                column: "ProovedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Recepcion_UsuarioId",
                table: "Recepcion",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesServicio_Cliente_ID",
                table: "ReportesServicio",
                column: "Cliente_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesServicio_Comprobante_ID",
                table: "ReportesServicio",
                column: "Comprobante_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Menu_IdRol",
                table: "Rol_Menu",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_User_Rol_IdRol",
                table: "User_Rol",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdEmpleado",
                table: "Usuario",
                column: "IdEmpleado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_Cliente_ID",
                table: "Ventas",
                column: "Cliente_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_Empleado_ID",
                table: "Ventas",
                column: "Empleado_ID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitasTecnicas_Cliente_ID",
                table: "VisitasTecnicas",
                column: "Cliente_ID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitasTecnicas_Empleado_ID",
                table: "VisitasTecnicas",
                column: "Empleado_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropTable(
                name: "EventosCalendario");

            migrationBuilder.DropTable(
                name: "InformesTecnicos");

            migrationBuilder.DropTable(
                name: "Mantenimientos");

            migrationBuilder.DropTable(
                name: "Recargas");

            migrationBuilder.DropTable(
                name: "Rol_Menu");

            migrationBuilder.DropTable(
                name: "User_Rol");

            migrationBuilder.DropTable(
                name: "VisitasTecnicas");

            migrationBuilder.DropTable(
                name: "Extintores");

            migrationBuilder.DropTable(
                name: "ReportesServicio");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Recepcion");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "ComprobantesServicio");

            migrationBuilder.DropTable(
                name: "Proovedor");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Empleado");
        }
    }
}
