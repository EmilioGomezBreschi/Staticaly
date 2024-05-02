using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Staticaly.Server.ModelsMigrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    PermisoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.PermisoID);
                });

            migrationBuilder.CreateTable(
                name: "Rangos",
                columns: table => new
                {
                    RangoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRango = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PuntosMin = table.Column<int>(type: "int", nullable: false),
                    PuntosMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rangos", x => x.RangoID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "TiposEquipos",
                columns: table => new
                {
                    TipoEquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposEquipos", x => x.TipoEquipoID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RolID = table.Column<int>(type: "int", nullable: false),
                    RangoID = table.Column<int>(type: "int", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    VerificationToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                    table.ForeignKey(
                        name: "FK_Usuarios_Rangos_RangoID",
                        column: x => x.RangoID,
                        principalTable: "Rangos",
                        principalColumn: "RangoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolID",
                        column: x => x.RolID,
                        principalTable: "Roles",
                        principalColumn: "RolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TipoEquipoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.EquipoID);
                    table.ForeignKey(
                        name: "FK_Equipos_TiposEquipos_TipoEquipoID",
                        column: x => x.TipoEquipoID,
                        principalTable: "TiposEquipos",
                        principalColumn: "TipoEquipoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ejercicios",
                columns: table => new
                {
                    EjerciciosID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publicado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timer = table.Column<int>(type: "int", nullable: false),
                    PuntosAquitar = table.Column<int>(type: "int", nullable: false),
                    permitirMasDeUnaRespuesta = table.Column<bool>(type: "bit", nullable: false),
                    PuntosMin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejercicios", x => x.EjerciciosID);
                    table.ForeignKey(
                        name: "FK_Ejercicios_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Publicaciones",
                columns: table => new
                {
                    PublicacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForoID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Reportes = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicaciones", x => x.PublicacionID);
                    table.ForeignKey(
                        name: "FK_Publicaciones_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RespuestasCuestionarios",
                columns: table => new
                {
                    RespuestasCuestionarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuestionarioID = table.Column<int>(type: "int", nullable: false),
                    PreguntaID = table.Column<int>(type: "int", nullable: false),
                    RespuestaID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasCuestionarios", x => x.RespuestasCuestionarioID);
                    table.ForeignKey(
                        name: "FK_RespuestasCuestionarios_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cuestionarios",
                columns: table => new
                {
                    CuestionarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publicado = table.Column<bool>(type: "bit", nullable: false),
                    Respuestas = table.Column<int>(type: "int", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RespuestasMaximas = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuestionarios", x => x.CuestionarioID);
                    table.ForeignKey(
                        name: "FK_Cuestionarios_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cuestionarios_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosEquipos",
                columns: table => new
                {
                    UsuarioEquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    PermisoID = table.Column<int>(type: "int", nullable: false),
                    FechaUnir = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosEquipos", x => x.UsuarioEquipoID);
                    table.ForeignKey(
                        name: "FK_UsuariosEquipos_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosEquipos_Permisos_PermisoID",
                        column: x => x.PermisoID,
                        principalTable: "Permisos",
                        principalColumn: "PermisoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosEquipos_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EjerciciosPreguntas",
                columns: table => new
                {
                    EjerciciosPreguntasID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EjercicioID = table.Column<int>(type: "int", nullable: false),
                    Pregunta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjerciciosPreguntas", x => x.EjerciciosPreguntasID);
                    table.ForeignKey(
                        name: "FK_EjerciciosPreguntas_Ejercicios_EjercicioID",
                        column: x => x.EjercicioID,
                        principalTable: "Ejercicios",
                        principalColumn: "EjerciciosID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    ComentarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicacionID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Calificacion = table.Column<float>(type: "real", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reportes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.ComentarioID);
                    table.ForeignKey(
                        name: "FK_Comentarios_Publicaciones_PublicacionID",
                        column: x => x.PublicacionID,
                        principalTable: "Publicaciones",
                        principalColumn: "PublicacionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    PreguntaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuestionarioID = table.Column<int>(type: "int", nullable: false),
                    Pregunta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", x => x.PreguntaID);
                    table.ForeignKey(
                        name: "FK_Preguntas_Cuestionarios_CuestionarioID",
                        column: x => x.CuestionarioID,
                        principalTable: "Cuestionarios",
                        principalColumn: "CuestionarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EjerciciosRespuestas",
                columns: table => new
                {
                    EjerciciosRespuestasID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    EjercicioID = table.Column<int>(type: "int", nullable: false),
                    EjercicioPreguntaID = table.Column<int>(type: "int", nullable: false),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correcta = table.Column<bool>(type: "bit", nullable: true),
                    Puntos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjerciciosRespuestas", x => x.EjerciciosRespuestasID);
                    table.ForeignKey(
                        name: "FK_EjerciciosRespuestas_EjerciciosPreguntas_EjercicioPreguntaID",
                        column: x => x.EjercicioPreguntaID,
                        principalTable: "EjerciciosPreguntas",
                        principalColumn: "EjerciciosPreguntasID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EjerciciosRespuestas_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    CalificacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComentarioID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Calificacion = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.CalificacionID);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Comentarios_ComentarioID",
                        column: x => x.ComentarioID,
                        principalTable: "Comentarios",
                        principalColumn: "ComentarioID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpcionesCuestionario",
                columns: table => new
                {
                    OpcionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreguntaID = table.Column<int>(type: "int", nullable: false),
                    Opcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpcionesCuestionario", x => x.OpcionID);
                    table.ForeignKey(
                        name: "FK_OpcionesCuestionario_Preguntas_PreguntaID",
                        column: x => x.PreguntaID,
                        principalTable: "Preguntas",
                        principalColumn: "PreguntaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permisos",
                columns: new[] { "PermisoID", "Nombre" },
                values: new object[,]
                {
                    { 1, "Administrar" },
                    { 2, "Visualizar" },
                    { 3, "Editar" }
                });

            migrationBuilder.InsertData(
                table: "Rangos",
                columns: new[] { "RangoID", "NombreRango", "PuntosMax", "PuntosMin" },
                values: new object[,]
                {
                    { 1, "Nuevo", 99, 0 },
                    { 2, "Viajero", 5000, 100 },
                    { 3, "Explorador", 10000, 5001 },
                    { 4, "Navegante", 15000, 10001 },
                    { 5, "Analista", 20000, 15001 },
                    { 6, "Estadistico", 25000, 20001 },
                    { 7, "Coordinador", 30000, 25001 },
                    { 8, "Arquitecto", 35000, 30001 },
                    { 9, "Maestro", 40000, 35001 },
                    { 10, "Guardian", 100000, 40001 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RolID", "RolName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Estudiante" },
                    { 3, "Docente" }
                });

            migrationBuilder.InsertData(
                table: "TiposEquipos",
                columns: new[] { "TipoEquipoID", "Nombre" },
                values: new object[,]
                {
                    { 1, "Publico" },
                    { 2, "Estudiante" },
                    { 3, "Docente" }
                });

            migrationBuilder.InsertData(
                table: "Equipos",
                columns: new[] { "EquipoID", "Descripcion", "Nombre", "TipoEquipoID" },
                values: new object[] { 1, null, "Foro Publico", 1 });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "UsuarioID", "Apellido", "Email", "EmailVerified", "Imagen", "Nombre", "Password", "Puntos", "RangoID", "RolID", "VerificationToken" },
                values: new object[] { 1, "Admin", "staticaly.services@gmail.com", true, null, "Admin", "$2a$12$wM8vSxJF5IO27LsmTeheheB.durdm4GUJp4RD9pLon4/fYTlR6mbS", 100000, 10, 1, null });

            migrationBuilder.InsertData(
                table: "Ejercicios",
                columns: new[] { "EjerciciosID", "Descripcion", "EquipoID", "FechaCierre", "Publicado", "PuntosAquitar", "PuntosMin", "Timer", "Titulo", "UsuarioID", "permitirMasDeUnaRespuesta" },
                values: new object[] { 1, "En base a estos ejercicios sera el rango que tengas dentro de la plataforma, hay un total de 100 ejercicios los cuales van a ir incrementando su dificultad conforme vayas avanzando, por cada respuesta incorrecta se restaran puntos y por cada segundo que pase se restaran puntos, por lo que es importante que contestes lo mas rapido posible y de manera correcta.", 1, null, true, 100, 100, 1000, "Ejercicios Publicos", 1, true });

            migrationBuilder.InsertData(
                table: "EjerciciosPreguntas",
                columns: new[] { "EjerciciosPreguntasID", "EjercicioID", "Imagen", "Pregunta", "Puntos", "Respuesta" },
                values: new object[,]
                {
                    { 1, 1, null, "Media (Promedio) Datos: 15, 20, 25, 30, 35.", 1000, "25" },
                    { 2, 1, null, "Mediana Datos: 8, 4, 6, 12, 10", 1000, "8" },
                    { 3, 1, null, "Moda Datos: 7, 4, 7, 9, 2, 4.", 1000, "7" },
                    { 4, 1, null, "Varianza Datos: 5, 10, 15, 20, 25. ", 1000, "50" },
                    { 5, 1, null, "5.	Desviación Estándar Datos: 9, 12, 15, 18, 21", 1000, "7.07" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_ComentarioID",
                table: "Calificaciones",
                column: "ComentarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_UsuarioID",
                table: "Calificaciones",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PublicacionID",
                table: "Comentarios",
                column: "PublicacionID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioID",
                table: "Comentarios",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Cuestionarios_EquipoID",
                table: "Cuestionarios",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_Cuestionarios_UsuarioID",
                table: "Cuestionarios",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Ejercicios_UsuarioID",
                table: "Ejercicios",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosPreguntas_EjercicioID",
                table: "EjerciciosPreguntas",
                column: "EjercicioID");

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosRespuestas_EjercicioPreguntaID",
                table: "EjerciciosRespuestas",
                column: "EjercicioPreguntaID");

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosRespuestas_UsuarioID",
                table: "EjerciciosRespuestas",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_TipoEquipoID",
                table: "Equipos",
                column: "TipoEquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_OpcionesCuestionario_PreguntaID",
                table: "OpcionesCuestionario",
                column: "PreguntaID");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_CuestionarioID",
                table: "Preguntas",
                column: "CuestionarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_UsuarioID",
                table: "Publicaciones",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasCuestionarios_UsuarioID",
                table: "RespuestasCuestionarios",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RangoID",
                table: "Usuarios",
                column: "RangoID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolID",
                table: "Usuarios",
                column: "RolID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEquipos_EquipoID",
                table: "UsuariosEquipos",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEquipos_PermisoID",
                table: "UsuariosEquipos",
                column: "PermisoID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEquipos_UsuarioID",
                table: "UsuariosEquipos",
                column: "UsuarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "EjerciciosRespuestas");

            migrationBuilder.DropTable(
                name: "OpcionesCuestionario");

            migrationBuilder.DropTable(
                name: "RespuestasCuestionarios");

            migrationBuilder.DropTable(
                name: "UsuariosEquipos");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "EjerciciosPreguntas");

            migrationBuilder.DropTable(
                name: "Preguntas");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Publicaciones");

            migrationBuilder.DropTable(
                name: "Ejercicios");

            migrationBuilder.DropTable(
                name: "Cuestionarios");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "TiposEquipos");

            migrationBuilder.DropTable(
                name: "Rangos");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
