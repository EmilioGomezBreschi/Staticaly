using Staticaly.Server.Models;
using Staticaly.Server.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(
    builder =>
    {
      builder.WithOrigins("http://localhost:5197")
            .AllowAnyHeader()
            .AllowAnyMethod();
    }));

var connectionString = builder.Configuration.GetConnectionString("StaticalyContext");
builder.Services.AddSqlServer<StaticalyContext>(connectionString);
builder.Services.AddScoped<IEmailService, EmailService>(); // Registro de EmailService como un servicio
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new() { Title = "Staticaly.Server", Version = "v1" });
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Staticaly.Server v1"));

#region Entry Points Email
app.MapPost("/Email", (EmailDTO request, IEmailService emailService) =>
{
  emailService.SendEmail(request);
  return Results.Ok();
});
#endregion

var userGroup = app.MapGroup("/users").WithParameterValidation();

#region Entry Points Users

// Obtener todos los usuarios
userGroup.MapGet("/", async (StaticalyContext context) =>
{
  var usuarios = await context.Usuarios
                              .Include(u => u.Rol)
                              .Include(u => u.Rango)
                              .AsNoTracking()
                              .ToListAsync();

  return usuarios.Any() ? Results.Ok(usuarios) : Results.NotFound();
});

// Obtener el correo electrónico del usuario por su ID
userGroup.MapGet("/byEmail/{id}/email", async (int id, StaticalyContext context) =>
{
  var user = await context.Usuarios.FindAsync(id);
  return user != null ? Results.Ok(user.Email) : Results.NotFound();
});

// Obtener usuario por correo electrónico
userGroup.MapGet("/byEmail/{email}", async (string email, StaticalyContext context) =>
{
  var user = await context.Usuarios
                          .Include(u => u.Rol)
                          .Include(u => u.Rango)
                          .AsNoTracking()
                          .FirstOrDefaultAsync(u => u.Email == email);

  return user != null ? Results.Ok(user) : Results.NotFound();
});

// Obtener usuario por su ID
userGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var user = await context.Usuarios
                          .Include(u => u.Rol)
                          .Include(u => u.Rango)
                          .FirstOrDefaultAsync(u => u.UsuarioID == id);

  return user != null ? Results.Ok(user) : Results.NotFound();
});

// Crear usuario
userGroup.MapPost("/", async (StaticalyContext context, User user) =>
{
  context.Usuarios.Add(user);
  await context.SaveChangesAsync();
  return Results.CreatedAtRoute("GetUsers", new { id = user.UsuarioID }, user);
}).WithName("GetUsers");

// Actualizar usuario
userGroup.MapPut("/{id}", async (StaticalyContext context, int id, User Updateduser) =>
{
  var user = await context.Usuarios.FindAsync(id);
  if (user == null)
  {
    return Results.NotFound();
  }

  context.Entry(user).CurrentValues.SetValues(Updateduser);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Verificar correo electrónico del usuario
userGroup.MapPut("/verificar/{rawtoken}", async (string rawtoken, StaticalyContext context) =>
{
  try
  {
    string token = HttpUtility.UrlDecode(rawtoken);
    var user = await context.Usuarios.FirstOrDefaultAsync(u => u.VerificationToken == token);
    if (user == null)
    {
      return Results.NotFound();
    }

    user.EmailVerified = true;
    user.VerificationToken = null;
    await context.SaveChangesAsync();

    return Results.NoContent();
  }
  catch (Exception ex)
  {
    Console.WriteLine("Error al verificar el correo electrónico: " + ex.Message);
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

// Cambiar contraseña del usuario
userGroup.MapPut("/cambiarcontrasena/{id}/{contrasenanueva}", async (string contrasenanueva, int id, StaticalyContext context) =>
{
  try
  {
    var user = await context.Usuarios.FindAsync(id);
    if (user == null)
    {
      return Results.NotFound();
    }

    user.Password = contrasenanueva;
    await context.SaveChangesAsync();

    return Results.Ok();
  }
  catch (Exception ex)
  {
    Console.WriteLine("Error al cambiar la contraseña: " + ex.Message);
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

// Actualizar rango del usuario
userGroup.MapPut("/rango/{id}/{puntos}", async (int id, int puntos, StaticalyContext context) =>
{
  try
  {
    var user = await context.Usuarios.FindAsync(id);
    if (user == null)
    {
      return Results.NotFound();
    }

    user.RangoID = await context.Rangos
                                .Where(rango => rango.PuntosMin <= puntos && rango.PuntosMax >= puntos)
                                .Select(rango => rango.RangoID)
                                .FirstOrDefaultAsync();
    await context.SaveChangesAsync();

    return Results.Ok();
  }
  catch (Exception ex)
  {
    Console.WriteLine("Error al cambiar el rango: " + ex.Message);
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

// Eliminar usuario
userGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var user = await context.Usuarios.FindAsync(id);
  if (user == null)
  {
    return Results.NotFound();
  }

  context.Usuarios.Remove(user);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Verificar token de usuario
userGroup.MapPut("/verifytoken/{token}", async (string token, StaticalyContext context) =>
{
  try
  {
    var user = await context.Usuarios.FirstOrDefaultAsync(user => user.VerificationToken == token);
    if (user == null)
    {
      return Results.NotFound();
    }

    user.EmailVerified = true;
    user.VerificationToken = null;
    await context.SaveChangesAsync();

    return Results.NoContent();
  }
  catch (Exception ex)
  {
    Console.WriteLine("Error al verificar el token del correo electrónico: " + ex.Message);
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

// Update rol usuario docente
userGroup.MapPut("/updaterol/{email}", async (string email, StaticalyContext context) =>
{
  try
  {
    var user = await context.Usuarios.FirstOrDefaultAsync(user => user.Email == email);
    if (user == null)
    {
      return Results.NotFound();
    }
    user.RolID = 3;
    await context.SaveChangesAsync();

    return Results.NoContent();
  }
  catch (Exception ex)
  {
    Console.WriteLine("Error al actualizar el rol del usuario: " + ex.Message);
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

//Delete foto de usuario
userGroup.MapDelete("/foto/{email}", async (string email, StaticalyContext context) =>
{
  try
  {
    var user = await context.Usuarios.FirstOrDefaultAsync(user => user.Email == email);
    if (user == null)
    {
      return Results.NotFound();
    }
    user.Imagen = null;
    await context.SaveChangesAsync();

    return Results.NoContent();
  }
  catch (Exception ex)
  {
    Console.WriteLine("Error al eliminar la foto del usuario: " + ex.Message);
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

//Update foto de usuario
userGroup.MapPut("/foto/", async (StaticalyContext context, User user) =>
{
  try
  {
    var userToUpdate = await context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioID == user.UsuarioID);
    if (userToUpdate == null)
    {
      return Results.NotFound();
    }
    userToUpdate.Imagen = user.Imagen;
    await context.SaveChangesAsync();

    return Results.NoContent();
  }
  catch (Exception ex)
  {
    Console.WriteLine("Error al actualizar la foto del usuario: " + ex.Message);
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

#endregion

var EquipoGroup = app.MapGroup("/equipos").WithParameterValidation();

#region Entry Points Equipos

// Obtener todos los equipos
EquipoGroup.MapGet("/", async (StaticalyContext context) =>
{
  var equipos = await context.Equipos
                          .Include(e => e.TipoEquipo)
                          .AsNoTracking()
                          .ToListAsync();

  return equipos.Any() ? Results.Ok(equipos) : Results.NotFound();
});

// Obtener equipo por ID
EquipoGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var equipo = await context.Equipos
                            .Include(e => e.TipoEquipo)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(e => e.EquipoID == id);

  return equipo != null ? Results.Ok(equipo) : Results.NotFound();
}).Produces<Equipos>();

// Borrar equipo por ID
EquipoGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var equipo = await context.Equipos.FindAsync(id);
  if (equipo == null)
  {
    return Results.NotFound();
  }

  context.Equipos.Remove(equipo);
  await context.SaveChangesAsync();

  return Results.StatusCode(204);
});

// Actualizar equipo por ID
EquipoGroup.MapPut("/{id}", async (StaticalyContext context, int id, Equipos equipo) =>
{
  var equipoToUpdate = await context.Equipos.FindAsync(id);
  if (equipoToUpdate == null)
  {
    return Results.NotFound();
  }
  // Actualiza las propiedades del equipoToUpdate con los valores proporcionados
  equipoToUpdate.Nombre = equipo.Nombre;
  equipoToUpdate.Descripcion = equipo.Descripcion;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Equipos>();

// Crear equipo
EquipoGroup.MapPost("/", async (StaticalyContext context, Equipos equipo) =>
{
  context.Equipos.Add(equipo);
  await context.SaveChangesAsync();
  return Results.Created($"/equipos/{equipo.EquipoID}", equipo);
}).WithName("GetEquipos").Produces<Equipos>();

#endregion

var UsuariosEquiposGroup = app.MapGroup("/usuariosequipos").WithParameterValidation();

# region Entry Points UsuariosEquipos

// Agregar usuario a un equipo
UsuariosEquiposGroup.MapPost("/", async (StaticalyContext context, UsuariosEquipos usuariosEquipos) =>
{
  context.UsuariosEquipos.Add(usuariosEquipos);
  await context.SaveChangesAsync();
  return Results.Created($"/usuariosequipos/{usuariosEquipos.UsuarioEquipoID}", usuariosEquipos);
}).Produces<UsuariosEquipos>();

// Eliminar usuario de un equipo por ID
UsuariosEquiposGroup.MapDelete("/{usuarioid}/{grupoid}", async (StaticalyContext context, int usuarioid, int grupoid) =>
{
  var usuarioEquipo = await context.UsuariosEquipos
                                  .FirstOrDefaultAsync(ue => ue.UsuarioID == usuarioid && ue.EquipoID == grupoid);
  if (usuarioEquipo == null)
  {
    return Results.NotFound();
  }

  context.UsuariosEquipos.Remove(usuarioEquipo);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Actualizar permiso de usuario en un equipo
UsuariosEquiposGroup.MapPut("/{id}/{permisoID}", async (StaticalyContext context, int id, int permisoID) =>
{
  var usuarioEquipoToUpdate = await context.UsuariosEquipos.FindAsync(id);
  if (usuarioEquipoToUpdate == null)
  {
    return Results.NotFound();
  }

  usuarioEquipoToUpdate.PermisoID = permisoID;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<UsuariosEquipos>();

// Obtener todos los equipos de un usuario
UsuariosEquiposGroup.MapGet("/byUsuario/{id}", async (StaticalyContext context, int id) =>
{
  var usuariosEquipos = await context.UsuariosEquipos
                                      .Include(ue => ue.Usuario)
                                      .Include(ue => ue.Equipo)
                                      .Include(ue => ue.Permiso)
                                      .AsNoTracking()
                                      .Where(ue => ue.UsuarioID == id)
                                      .ToListAsync();

  return Results.Ok(usuariosEquipos);
});

// Obtener todos los usuarios de un equipo
UsuariosEquiposGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var usuariosEquipos = await context.UsuariosEquipos
                                      .Include(ue => ue.Usuario)
                                        .ThenInclude(u => u.Rango)
                                      .Include(ue => ue.Equipo)
                                      .Include(ue => ue.Permiso)
                                      .AsNoTracking()
                                      .Where(ue => ue.EquipoID == id)
                                      .ToListAsync();

  return Results.Ok(usuariosEquipos); // Devolver una respuesta exitosa con la lista de usuarios, incluso si está vacía
});


#endregion

var publicacionesGroup = app.MapGroup("/publicaciones").WithParameterValidation();

#region Entry Points Publicaciones

// Crear publicacion
publicacionesGroup.MapPost("/", async (StaticalyContext context, Publicaciones publicacion) =>
{
  context.Publicaciones.Add(publicacion);
  await context.SaveChangesAsync();
  return Results.Created($"/publicaciones/{publicacion.PublicacionID}", publicacion);
}).Produces<Publicaciones>();

// Obtener publicacion por ID
publicacionesGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var publicacion = await context.Publicaciones
                              .Include(p => p.Usuario)
                              .ThenInclude(u => u.Rango)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(p => p.PublicacionID == id);

  return publicacion != null ? Results.Ok(publicacion) : Results.NotFound();
}).Produces<Publicaciones>();

// Obtener publicaciones en equipo paginadas
publicacionesGroup.MapGet("/byEquipo/{id}", async (StaticalyContext context, int id, int page = 1, int pageSize = 5) =>
{
  var skipAmount = (page - 1) * pageSize;

  var publicaciones = await context.Publicaciones
      .Include(p => p.Usuario)
      .ThenInclude(u => u.Rango)
      .AsNoTracking()
      .Where(p => p.ForoID == id)
      .OrderByDescending(p => p.Fecha) // Ordena por Fecha descendente
      .Skip(skipAmount)
      .Take(pageSize)
      .ToListAsync();

  foreach (var publicacion in publicaciones)
  {
    publicacion.Reportes = await context.Reportes
                                      .Where(r => r.PublicacionID == publicacion.PublicacionID)
                                      .CountAsync();
  }

  var totalPublicaciones = await context.Publicaciones
      .Where(p => p.ForoID == id)
      .CountAsync();

  var totalPages = (int)Math.Ceiling(totalPublicaciones / (double)pageSize);

  return Results.Ok(new
  {
    Publicaciones = publicaciones,
    TotalPaginas = totalPages
  });
});

// Obtener publicaciones de equipo sin paginar
publicacionesGroup.MapGet("/byEquipo/{id}/all", async (StaticalyContext context, int id) =>
{
  var publicaciones = await context.Publicaciones
      .Include(p => p.Usuario)
      .ThenInclude(u => u.Rango)
      .AsNoTracking()
      .Where(p => p.ForoID == id)
      .ToListAsync();

  foreach (var publicacion in publicaciones)
  {
    publicacion.Reportes = await context.Reportes
                                      .Where(r => r.PublicacionID == publicacion.PublicacionID)
                                      .CountAsync();
  }

  return Results.Ok(publicaciones);
});



// Obtener publicaciones de equipo por parte del título paginadas
publicacionesGroup.MapGet("/byEquipo/{id}/{titulo}", async (StaticalyContext context, int id, string titulo, int page = 1, int pageSize = 5) =>
{
  var skipAmount = (page - 1) * pageSize;

  var publicaciones = await context.Publicaciones
      .Include(p => p.Usuario)
      .ThenInclude(u => u.Rango)
      .AsNoTracking()
      .Where(p => p.ForoID == id && p.Titulo != null && (p.Titulo.Contains(titulo) || p.Titulo.Contains(titulo.ToLower()) || p.Titulo.Contains(titulo.ToUpper()) || p.Contenido.Contains(titulo) || p.Contenido.Contains(titulo.ToLower()) || p.Contenido.Contains(titulo.ToUpper()) || p.Usuario.Nombre.Contains(titulo) || p.Usuario.Nombre.Contains(titulo.ToLower()) || p.Usuario.Nombre.Contains(titulo.ToUpper())))
      .OrderByDescending(p => p.Fecha) // Ordena por Fecha descendente
      .Skip(skipAmount)
      .Take(pageSize)
      .ToListAsync();

  var totalPublicaciones = await context.Publicaciones
      .Where(p => p.ForoID == id && p.Titulo != null && (p.Titulo.Contains(titulo) || p.Titulo.Contains(titulo.ToLower()) || p.Titulo.Contains(titulo.ToUpper()) || p.Contenido.Contains(titulo) || p.Contenido.Contains(titulo.ToLower()) || p.Contenido.Contains(titulo.ToUpper()) || p.Usuario.Nombre.Contains(titulo) || p.Usuario.Nombre.Contains(titulo.ToLower()) || p.Usuario.Nombre.Contains(titulo.ToUpper())))
      .CountAsync();

  foreach (var publicacion in publicaciones)
  {
    publicacion.Reportes = await context.Reportes
                                      .Where(r => r.PublicacionID == publicacion.PublicacionID)
                                      .CountAsync();
  }

  var totalPages = (int)Math.Ceiling(totalPublicaciones / (double)pageSize);

  return Results.Ok(new
  {
    Publicaciones = publicaciones,
    TotalPaginas = totalPages
  });
});

// Editar publicacion
publicacionesGroup.MapPut("/{id}", async (StaticalyContext context, int id, Publicaciones publicacion) =>
{
  var publicacionToUpdate = await context.Publicaciones.FindAsync(id);
  if (publicacionToUpdate == null)
  {
    return Results.NotFound();
  }

  publicacionToUpdate.Titulo = publicacion.Titulo;
  publicacionToUpdate.Contenido = publicacion.Contenido;
  publicacionToUpdate.Imagen = publicacion.Imagen;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Publicaciones>();

// Actualizar reportes de publicación
publicacionesGroup.MapPut("/reportes/{id}/{reportes}", async (StaticalyContext context, int id, int reportes) =>
{
  var publicacionToUpdate = await context.Publicaciones.FindAsync(id);
  if (publicacionToUpdate == null)
  {
    return Results.NotFound();
  }

  publicacionToUpdate.Reportes = reportes;

  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Eliminar publicacion
publicacionesGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var publicacion = await context.Publicaciones.FindAsync(id);
  if (publicacion == null)
  {
    return Results.NotFound();
  }

  context.Publicaciones.Remove(publicacion);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var comentariosGroup = app.MapGroup("/comentarios").WithParameterValidation();

#region Entry Points Comentarios

// Crear comentario
comentariosGroup.MapPost("/", async (StaticalyContext context, Comentarios comentario) =>
{
  context.Comentarios.Add(comentario);
  await context.SaveChangesAsync();
  return Results.Created($"/comentarios/{comentario.ComentarioID}", comentario);
}).Produces<Comentarios>();

// Obtener comentarios de una publicación
comentariosGroup.MapGet("/byPublicacion/{id}", async (StaticalyContext context, int id) =>
{
  var comentarios = await context.Comentarios
                                  .Include(c => c.Usuario)
                                  .ThenInclude(u => u.Rango)
                                  .AsNoTracking()
                                  .Where(c => c.PublicacionID == id)
                                  .ToListAsync();

  // Calcular el promedio de calificaciones para cada comentario
  foreach (var comentario in comentarios)
  {
    var calificaciones = await context.Calificaciones
                                      .Where(c => c.ComentarioID == comentario.ComentarioID)
                                      .Select(c => c.Calificacion)
                                      .ToListAsync();

    if (calificaciones.Any())
    {
      comentario.Calificacion = calificaciones.Average();
    }

    // Obtener el número de reportes para cada comentario
    comentario.Reportes = await context.Reportes
                                      .Where(r => r.ComentarioID == comentario.ComentarioID)
                                      .CountAsync();

  }

  // Ordenar los comentarios por calificación descendente
  comentarios = comentarios.OrderByDescending(c => c.Calificacion).ToList();

  return Results.Ok(comentarios);
});

// Obtener comentarios de un Foro
comentariosGroup.MapGet("/byForo/{id}", async (StaticalyContext context, int id) =>
{
  var comentarios = await context.Comentarios
                                  .Include(c => c.Usuario)
                                  .ThenInclude(u => u.Rango)
                                  .AsNoTracking()
                                  .Where(c => c.Publicacion.ForoID == id)
                                  .ToListAsync();

  // Calcular el promedio de calificaciones para cada comentario
  foreach (var comentario in comentarios)
  {
    var calificaciones = await context.Calificaciones
                                      .Where(c => c.ComentarioID == comentario.ComentarioID)
                                      .Select(c => c.Calificacion)
                                      .ToListAsync();

    if (calificaciones.Any())
    {
      comentario.Calificacion = calificaciones.Average();
    }

    // Obtener el número de reportes para cada comentario
    comentario.Reportes = await context.Reportes
                                      .Where(r => r.ComentarioID == comentario.ComentarioID)
                                      .CountAsync();

  }

  // Ordenar los comentarios por calificación descendente
  comentarios = comentarios.OrderByDescending(c => c.Calificacion).ToList();

  return Results.Ok(comentarios);
});

// Editar comentario
comentariosGroup.MapPut("/{id}", async (StaticalyContext context, int id, Comentarios comentario) =>
{
  var comentarioToUpdate = await context.Comentarios.FindAsync(id);
  if (comentarioToUpdate == null)
  {
    return Results.NotFound();
  }

  comentarioToUpdate.Contenido = comentario.Contenido;
  comentarioToUpdate.Imagen = comentario.Imagen;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Comentarios>();

// Eliminar comentario
comentariosGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var comentario = await context.Comentarios.FindAsync(id);
  if (comentario == null)
  {
    return Results.NotFound();
  }

  context.Comentarios.Remove(comentario);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var calificacionesGroup = app.MapGroup("/calificaciones").WithParameterValidation();

#region Entry Points Calificaciones

// Crear calificacion
calificacionesGroup.MapPost("/", async (StaticalyContext context, Calificaciones calificacion) =>
{
  context.Calificaciones.Add(calificacion);
  await context.SaveChangesAsync();
  return Results.Created($"/calificaciones/{calificacion.CalificacionID}", calificacion);
}).Produces<Calificaciones>();

// editar calificacion by comentario y usuario
calificacionesGroup.MapPut("/{comentarioID}/{usuarioID}", async (StaticalyContext context, int comentarioID, int usuarioID, Calificaciones calificacion) =>
{
  var calificacionToUpdate = await context.Calificaciones
                                      .FirstOrDefaultAsync(c => c.ComentarioID == comentarioID && c.UsuarioID == usuarioID);
  if (calificacionToUpdate == null)
  {
    return Results.NotFound();
  }

  calificacionToUpdate.Calificacion = calificacion.Calificacion;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Calificaciones>();

// Obtener calificaciones por comentario y usuario

calificacionesGroup.MapGet("/byComentario/{comentarioID}/{UsuarioID}", async (StaticalyContext context, int comentarioID, int UsuarioID) =>
{
  var calificaciones = await context.Calificaciones
      .Where(c => c.ComentarioID == comentarioID && c.UsuarioID == UsuarioID)
      .ToListAsync();

  if (calificaciones.Any())
  {
    return Results.Ok(calificaciones);
  }
  else
  {
    return Results.NotFound();
  }
});

#endregion

var ReportesGroup = app.MapGroup("/reportes").WithParameterValidation();

#region Entry Points Reportes

// Crear reporte
ReportesGroup.MapPost("/", async (StaticalyContext context, Reportes reporte) =>
{
  context.Reportes.Add(reporte);
  await context.SaveChangesAsync();
  return Results.Created($"/reportes/{reporte.ReportesID}", reporte);
}).Produces<Reportes>();

// Obtener reportes por usuario
ReportesGroup.MapGet("/byUsuario/{id}", async (StaticalyContext context, int id) =>
{
  var reportes = await context.Reportes
                          .AsNoTracking()
                          .Where(r => r.UsuarioID == id)
                          .ToListAsync();

  return Results.Ok(reportes);
});

// Obtener reportes por publicacion
ReportesGroup.MapGet("/byPublicacion/{id}", async (StaticalyContext context, int id) =>
{
  var reportes = await context.Reportes
                          .Include(r => r.User)
                          .AsNoTracking()
                          .Where(r => r.PublicacionID == id)
                          .ToListAsync();

  return Results.Ok(reportes);
});


// Obtener conteo de reportes por comentario y por publicacion con más de 3 reportes por el ID del grupo
ReportesGroup.MapGet("/byGrupo/{id}", async (StaticalyContext context, int id) =>
{
  // Obtener el conteo de reportes por comentario
  var reportesComentarios = await context.Comentarios
  .Include(c => c.Publicacion)
      .Where(c => c.Publicacion.ForoID == id)
      .Select(c => new
      {
        c = c.ComentarioID,
        Reportes = context.Reportes.Count(r => r.ComentarioID == c.ComentarioID)
      })
      .Where(c => c.Reportes >= 3)
      .CountAsync(); // Cambiar a CountAsync para obtener el conteo directamente

  // Obtener el conteo de reportes por publicación
  var reportesPublicaciones = await context.Publicaciones
      .Where(p => p.ForoID == id)
      .Select(p => new
      {
        p = p.PublicacionID,
        Reportes = context.Reportes.Count(r => r.PublicacionID == p.PublicacionID)
      })
      .Where(p => p.Reportes >= 3)
      .CountAsync(); // Cambiar a CountAsync para obtener el conteo directamente

  return Results.Ok(new
  {
    ReportesGenerales = reportesComentarios + reportesPublicaciones
  });
});

// Obtener reportes por comentario
ReportesGroup.MapGet("/byComentario/{id}", async (StaticalyContext context, int id) =>
{
  var reportes = await context.Reportes
                          .Include(r => r.User)
                          .AsNoTracking()
                          .Where(r => r.ComentarioID == id)
                          .ToListAsync();

  return Results.Ok(reportes);
});

// Eliminar reporte por Publicacion y Usuario
ReportesGroup.MapDelete("/byPublicacion/{id}/{usuarioID}", async (StaticalyContext context, int id, int usuarioID) =>
{
  var reporte = await context.Reportes
                          .FirstOrDefaultAsync(r => r.PublicacionID == id && r.UsuarioID == usuarioID);
  if (reporte == null)
  {
    return Results.NotFound();
  }

  context.Reportes.Remove(reporte);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Eliminar reporte por Comentario y Usuario
ReportesGroup.MapDelete("/byComentario/{id}/{usuarioID}", async (StaticalyContext context, int id, int usuarioID) =>
{
  var reporte = await context.Reportes
                          .FirstOrDefaultAsync(r => r.ComentarioID == id && r.UsuarioID == usuarioID);
  if (reporte == null)
  {
    return Results.NotFound();
  }

  context.Reportes.Remove(reporte);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

//Eliminar todos los reportes de una publicacion
ReportesGroup.MapDelete("/byPublicacion/{id}", async (StaticalyContext context, int id) =>
{
  var reportes = await context.Reportes
                          .Where(r => r.PublicacionID == id)
                          .ToListAsync();

  if (reportes == null)
  {
    return Results.NotFound();
  }

  context.Reportes.RemoveRange(reportes);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

//Eliminar todos los reportes de un comentario
ReportesGroup.MapDelete("/byComentario/{id}", async (StaticalyContext context, int id) =>
{
  var reportes = await context.Reportes
                          .Where(r => r.ComentarioID == id)
                          .ToListAsync();

  if (reportes == null)
  {
    return Results.NotFound();
  }

  context.Reportes.RemoveRange(reportes);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var cuestionariosGroup = app.MapGroup("/cuestionarios").WithParameterValidation();

#region Entry Points Cuestionarios

// Crear cuestionario
cuestionariosGroup.MapPost("/", async (StaticalyContext context, Cuestionarios cuestionario) =>
{
  context.Cuestionarios.Add(cuestionario);
  await context.SaveChangesAsync();
  return Results.Created($"/cuestionarios/{cuestionario.CuestionarioID}", cuestionario);
}).Produces<Cuestionarios>();

// Obtener cuestionario por Equipo
cuestionariosGroup.MapGet("/byEquipo/{id}", async (StaticalyContext context, int id) =>
{
  var cuestionarios = await context.Cuestionarios
                                  .Include(c => c.Usuario)
                                  .Include(c => c.Equipo)
                                  .AsNoTracking()
                                  .Where(c => c.EquipoID == id)
                                  .ToListAsync();

  return Results.Ok(cuestionarios);
});

// Obtener cuestionario por ID
cuestionariosGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var cuestionario = await context.Cuestionarios
                              .Include(c => c.Usuario)
                              .Include(c => c.Equipo)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(c => c.CuestionarioID == id);

  return cuestionario != null ? Results.Ok(cuestionario) : Results.NotFound();
}).Produces<Cuestionarios>();

//Obtener cuestionario por usuario
cuestionariosGroup.MapGet("/byUsuario/{id}", async (StaticalyContext context, int id) =>
{
  var cuestionarios = await context.Cuestionarios
                                  .Include(c => c.Usuario)
                                  .Include(c => c.Equipo)
                                  .AsNoTracking()
                                  .Where(c => c.UsuarioID == id)
                                  .ToListAsync();

  return Results.Ok(cuestionarios);
});

// Eliminar cuestionario
cuestionariosGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var cuestionario = await context.Cuestionarios.FindAsync(id);
  if (cuestionario == null)
  {
    return Results.NotFound();
  }

  context.Cuestionarios.Remove(cuestionario);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Cambiar estado de publicación de cuestionario
cuestionariosGroup.MapPut("/publicado/{id}/{publicado}", async (StaticalyContext context, int id, bool publicado) =>
{
  var cuestionarioToUpdate = await context.Cuestionarios.FindAsync(id);
  if (cuestionarioToUpdate == null)
  {
    return Results.NotFound();
  }

  cuestionarioToUpdate.Publicado = publicado;

  await context.SaveChangesAsync();

  return Results.NoContent();
});

//Actualizar respuestas de cuestionario
cuestionariosGroup.MapPut("/respuestas/{id}", async (StaticalyContext context, int id) =>
{
  var cuestionarioToUpdate = await context.Cuestionarios.FindAsync(id);
  if (cuestionarioToUpdate == null)
  {
    return Results.NotFound();
  }

  cuestionarioToUpdate.Respuestas = cuestionarioToUpdate.Respuestas + 1;

  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Editar cuestionario

cuestionariosGroup.MapPut("/{id}", async (StaticalyContext context, int id, Cuestionarios cuestionario) =>
{
  var cuestionarioToUpdate = await context.Cuestionarios.FindAsync(id);
  if (cuestionarioToUpdate == null)
  {
    return Results.NotFound();
  }

  cuestionarioToUpdate.Titulo = cuestionario.Titulo;
  cuestionarioToUpdate.Descripcion = cuestionario.Descripcion;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Cuestionarios>();

#endregion

var preguntasGroup = app.MapGroup("/preguntas").WithParameterValidation();

#region Entry Points Preguntas

// Crear pregunta
preguntasGroup.MapPost("/", async (StaticalyContext context, Preguntas pregunta) =>
{
  context.Preguntas.Add(pregunta);
  await context.SaveChangesAsync();
  return Results.Created($"/preguntas/{pregunta.PreguntaID}", pregunta);
}).Produces<Preguntas>();

// Obtener preguntas por cuestionario
preguntasGroup.MapGet("/byCuestionario/{id}", async (StaticalyContext context, int id) =>
{
  var preguntas = await context.Preguntas
                              .Include(p => p.Cuestionario)
                              .AsNoTracking()
                              .Where(p => p.CuestionarioID == id)
                              .ToListAsync();

  return Results.Ok(preguntas);
});

// Editar pregunta
preguntasGroup.MapPut("/{id}", async (StaticalyContext context, int id, Preguntas pregunta) =>
{
  var preguntaToUpdate = await context.Preguntas.FindAsync(id);
  if (preguntaToUpdate == null)
  {
    return Results.NotFound();
  }

  preguntaToUpdate.Pregunta = pregunta.Pregunta;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Preguntas>();

// Eliminar pregunta
preguntasGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var pregunta = await context.Preguntas.FindAsync(id);
  if (pregunta == null)
  {
    return Results.NotFound();
  }

  context.Preguntas.Remove(pregunta);
  await context.SaveChangesAsync();

  return Results.NoContent();
});
#endregion

var opcionesCuestionarioGroup = app.MapGroup("/opcionescuestionario").WithParameterValidation();

#region Entry Points OpcionesCuestionario

// Crear opción de cuestionario
opcionesCuestionarioGroup.MapPost("/", async (StaticalyContext context, OpcionesCuestionario opcion) =>
{
  context.OpcionesCuestionario.Add(opcion);
  await context.SaveChangesAsync();
  return Results.Created($"/opcionescuestionario/{opcion.OpcionID}", opcion);
}).Produces<OpcionesCuestionario>();

// Obtener opciones de cuestionario por pregunta
opcionesCuestionarioGroup.MapGet("/byPregunta/{id}", async (StaticalyContext context, int id) =>
{
  var opciones = await context.OpcionesCuestionario
                              .Include(o => o.Pregunta)
                              .AsNoTracking()
                              .Where(o => o.PreguntaID == id)
                              .ToListAsync();

  return Results.Ok(opciones);
});

// Editar opción de cuestionario
opcionesCuestionarioGroup.MapPut("/{id}", async (StaticalyContext context, int id, OpcionesCuestionario opcion) =>
{
  var opcionToUpdate = await context.OpcionesCuestionario.FindAsync(id);
  if (opcionToUpdate == null)
  {
    return Results.NotFound();
  }

  opcionToUpdate.Opcion = opcion.Opcion;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<OpcionesCuestionario>();

// Eliminar opción de cuestionario
opcionesCuestionarioGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var opcion = await context.OpcionesCuestionario.FindAsync(id);
  if (opcion == null)
  {
    return Results.NotFound();
  }

  context.OpcionesCuestionario.Remove(opcion);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var respuestasCuestionarioGroup = app.MapGroup("/respuestascuestionario").WithParameterValidation();

#region Entry Points RespuestasCuestionario

// Crear respuesta de cuestionario
respuestasCuestionarioGroup.MapPost("/", async (StaticalyContext context, RespuestasCuestionario respuesta) =>
{
  context.RespuestasCuestionarios.Add(respuesta);
  await context.SaveChangesAsync();
  return Results.Created($"/respuestascuestionario/{respuesta.RespuestasCuestionarioID}", respuesta);
}).Produces<RespuestasCuestionario>();

// Obtener respuestas de cuestionario por usuario
respuestasCuestionarioGroup.MapGet("/byUsuario/{id}", async (StaticalyContext context, int id) =>
{
  var respuestas = await context.RespuestasCuestionarios
                              .Include(r => r.User)
                              .AsNoTracking()
                              .Where(r => r.UsuarioID == id)
                              .ToListAsync();

  return Results.Ok(respuestas);
});

//Obtener respuestas de cuestionario por cuestionario
respuestasCuestionarioGroup.MapGet("/byCuestionario/{id}", async (StaticalyContext context, int id) =>
{
  var respuestas = await context.RespuestasCuestionarios
                              .Include(r => r.User)
                              .AsNoTracking()
                              .Where(r => r.CuestionarioID == id)
                              .ToListAsync();

  return Results.Ok(respuestas);
});

// Eliminar respuesta de cuestionario
respuestasCuestionarioGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var respuesta = await context.RespuestasCuestionarios.FindAsync(id);
  if (respuesta == null)
  {
    return Results.NotFound();
  }

  context.RespuestasCuestionarios.Remove(respuesta);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var ejerciciosGroup = app.MapGroup("/ejercicios").WithParameterValidation();

#region Entry Points Ejercicios

// Crear ejercicio
ejerciciosGroup.MapPost("/", async (StaticalyContext context, Ejercicios ejercicio) =>
{
  context.Ejercicios.Add(ejercicio);
  await context.SaveChangesAsync();
  return Results.Created($"/ejercicios/{ejercicio.EjerciciosID}", ejercicio);
}).Produces<Ejercicios>();

// Obtener ejercicios por Equipo
ejerciciosGroup.MapGet("/byEquipo/{id}", async (StaticalyContext context, int id) =>
{
  var ejercicios = await context.Ejercicios
                              .Include(e => e.User)
                              .AsNoTracking()
                              .Where(e => e.EquipoID == id)
                              .ToListAsync();

  return Results.Ok(ejercicios);
});

// Obtener ejercicio por ID
ejerciciosGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var ejercicio = await context.Ejercicios.FindAsync(id);
  return ejercicio != null ? Results.Ok(ejercicio) : Results.NotFound();
}).Produces<Ejercicios>();

// Editar ejercicio
ejerciciosGroup.MapPut("/{id}", async (StaticalyContext context, int id, Ejercicios ejercicio) =>
{
  var ejercicioToUpdate = await context.Ejercicios.FindAsync(id);
  if (ejercicioToUpdate == null)
  {
    return Results.NotFound();
  }

  ejercicioToUpdate.Titulo = ejercicio.Titulo;
  ejercicioToUpdate.Descripcion = ejercicio.Descripcion;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Ejercicios>();

// Eliminar ejercicio
ejerciciosGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var ejercicio = await context.Ejercicios.FindAsync(id);
  if (ejercicio == null)
  {
    return Results.NotFound();
  }

  context.Ejercicios.Remove(ejercicio);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Cambiar estado de publicación de ejercicio
ejerciciosGroup.MapPut("/publicado/{id}/{publicado}", async (StaticalyContext context, int id, bool publicado) =>
{
  var ejercicioToUpdate = await context.Ejercicios.FindAsync(id);
  if (ejercicioToUpdate == null)
  {
    return Results.NotFound();
  }

  ejercicioToUpdate.Publicado = publicado;

  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var ejerciciosPreguntasGroup = app.MapGroup("/ejerciciospreguntas").WithParameterValidation();

#region Entry Points EjerciciosPreguntas

// Crear pregunta de ejercicio
ejerciciosPreguntasGroup.MapPost("/", async (StaticalyContext context, EjerciciosPreguntas pregunta) =>
{
  context.EjerciciosPreguntas.Add(pregunta);
  await context.SaveChangesAsync();
  return Results.Created($"/ejerciciospreguntas/{pregunta.EjerciciosPreguntasID}", pregunta);
}).Produces<EjerciciosPreguntas>();

// Obtener preguntas de ejercicio por ejercicio
ejerciciosPreguntasGroup.MapGet("/byEjercicio/{id}", async (StaticalyContext context, int id) =>
{
  var preguntas = await context.EjerciciosPreguntas
                              .Include(p => p.Ejercicios)
                              .AsNoTracking()
                              .Where(p => p.EjercicioID == id)
                              .ToListAsync();

  return Results.Ok(preguntas);
});

// Editar pregunta de ejercicio
ejerciciosPreguntasGroup.MapPut("/{id}", async (StaticalyContext context, int id, EjerciciosPreguntas pregunta) =>
{
  var preguntaToUpdate = await context.EjerciciosPreguntas.FindAsync(id);
  if (preguntaToUpdate == null)
  {
    return Results.NotFound();
  }

  preguntaToUpdate.Pregunta = pregunta.Pregunta;
  preguntaToUpdate.Imagen = pregunta.Imagen;
  preguntaToUpdate.Respuesta = pregunta.Respuesta;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<EjerciciosPreguntas>();

// Eliminar pregunta de ejercicio
ejerciciosPreguntasGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var pregunta = await context.EjerciciosPreguntas.FindAsync(id);
  if (pregunta == null)
  {
    return Results.NotFound();
  }

  context.EjerciciosPreguntas.Remove(pregunta);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var ejerciciosRespuestasGroup = app.MapGroup("/ejerciciosrespuestas").WithParameterValidation();

#region Entry Points EjerciciosRespuestas

// Crear respuesta de ejercicio
ejerciciosRespuestasGroup.MapPost("/", async (StaticalyContext context, EjerciciosRespuestas respuesta) =>
{
  context.EjerciciosRespuestas.Add(respuesta);
  await context.SaveChangesAsync();
  return Results.Created($"/ejerciciosrespuestas/{respuesta.EjerciciosRespuestasID}", respuesta);
}).Produces<EjerciciosRespuestas>();

// Obtener respuestas de ejercicio por usuario
ejerciciosRespuestasGroup.MapGet("/byUsuario/{id}", async (StaticalyContext context, int id) =>
{
  var respuestas = await context.EjerciciosRespuestas
                              .Include(r => r.User)
                              .Include(r => r.EjerciciosPreguntas)
                              .AsNoTracking()
                              .Where(r => r.UsuarioID == id)
                              .ToListAsync();

  return Results.Ok(respuestas);
});

// Eliminar respuesta de ejercicio
ejerciciosRespuestasGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var respuesta = await context.EjerciciosRespuestas.FindAsync(id);
  if (respuesta == null)
  {
    return Results.NotFound();
  }

  context.EjerciciosRespuestas.Remove(respuesta);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var proyectosGroup = app.MapGroup("/proyectos").WithParameterValidation();

#region Entry Points Proyectos

// Crear proyecto
proyectosGroup.MapPost("/", async (StaticalyContext context, Proyectos proyecto) =>
{
  context.Proyectos.Add(proyecto);
  await context.SaveChangesAsync();
  return Results.Created($"/proyectos/{proyecto.ProyectoID}", proyecto);
}).Produces<Proyectos>();

// Obtener proyectos por Equipo
proyectosGroup.MapGet("/byEquipo/{id}", async (StaticalyContext context, int id) =>
{
  var proyectos = await context.Proyectos
                              .Include(p => p.Usuario)
                              .AsNoTracking()
                              .Where(p => p.EquipoID == id)
                              .ToListAsync();

  return Results.Ok(proyectos);
});

// Obtener proyecto por ID
proyectosGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var proyecto = await context.Proyectos
                              .Include(p => p.Usuario)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(p => p.ProyectoID == id);

  return proyecto != null ? Results.Ok(proyecto) : Results.NotFound();
}).Produces<Proyectos>();

// Editar proyecto
proyectosGroup.MapPut("/{id}", async (StaticalyContext context, int id, Proyectos proyecto) =>
{
  var proyectoToUpdate = await context.Proyectos.FindAsync(id);
  if (proyectoToUpdate == null)
  {
    return Results.NotFound();
  }

  proyectoToUpdate.Titulo = proyecto.Titulo;
  proyectoToUpdate.Descripcion = proyecto.Descripcion;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<Proyectos>();

// Eliminar proyecto
proyectosGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var proyecto = await context.Proyectos.FindAsync(id);
  if (proyecto == null)
  {
    return Results.NotFound();
  }

  context.Proyectos.Remove(proyecto);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var SubProyectosGroup = app.MapGroup("/SubProyectos").WithParameterValidation();

#region Entry Points SubProyectos

// Crear subproyecto
SubProyectosGroup.MapPost("/", async (StaticalyContext context, SubProyectos subproyecto) =>
{
  context.SubProyectos.Add(subproyecto);
  await context.SaveChangesAsync();
  return Results.Created($"/SubProyectos/{subproyecto.SubproyectoID}", subproyecto);
}).Produces<SubProyectos>();

// Obtener SubProyectos por proyecto
SubProyectosGroup.MapGet("/byProyecto/{id}", async (StaticalyContext context, int id) =>
{
  var SubProyectos = await context.SubProyectos
                              .Include(sp => sp.Proyecto)
                              .AsNoTracking()
                              .Where(sp => sp.ProyectoID == id)
                              .ToListAsync();

  return Results.Ok(SubProyectos);
});

// Obtener SubProyecto por ID
SubProyectosGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  var subproyecto = await context.SubProyectos
                              .Include(sp => sp.Proyecto)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(sp => sp.SubproyectoID == id);

  return subproyecto != null ? Results.Ok(subproyecto) : Results.NotFound();
}).Produces<SubProyectos>();

// Editar subproyecto
SubProyectosGroup.MapPut("/{id}", async (StaticalyContext context, int id, SubProyectos subproyecto) =>
{
  var subproyectoToUpdate = await context.SubProyectos.FindAsync(id);
  if (subproyectoToUpdate == null)
  {
    return Results.NotFound();
  }

  subproyectoToUpdate.Titulo = subproyecto.Titulo;
  subproyectoToUpdate.Descripcion = subproyecto.Descripcion;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<SubProyectos>();

// Eliminar subproyecto
SubProyectosGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var subproyecto = await context.SubProyectos.FindAsync(id);
  if (subproyecto == null)
  {
    return Results.NotFound();
  }

  context.SubProyectos.Remove(subproyecto);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

// Cambiar estado de Edicion de subproyecto
SubProyectosGroup.MapPut("/editado/{id}/{editando}/{usuarioid}", async (StaticalyContext context, int id, bool editando, int usuarioID) =>
{
  var subproyectoToUpdate = await context.SubProyectos.FindAsync(id);
  if (subproyectoToUpdate == null)
  {
    return Results.NotFound();
  }

  subproyectoToUpdate.Editando = editando;
  subproyectoToUpdate.UsuarioID = usuarioID;

  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

var datosSubproyectoGroup = app.MapGroup("/datossubproyecto").WithParameterValidation();

#region Entry Points DatosSubProyectos

// Crear dato de subproyecto
datosSubproyectoGroup.MapPost("/", async (StaticalyContext context, DatosSubProyecto dato) =>
{
  context.DatosSubProyecto.Add(dato);
  await context.SaveChangesAsync();
  return Results.Created($"/datosSubProyectos/{dato.DatosSubProyectoID}", dato);
}).Produces<DatosSubProyecto>();

// Obtener datos de subproyecto por subproyecto
datosSubproyectoGroup.MapGet("/bySubproyecto/{id}", async (StaticalyContext context, int id) =>
{
  var datos = await context.DatosSubProyecto
                      .Include(d => d.Subproyecto)
                      .AsNoTracking()
                      .Where(d => d.SubproyectoID == id)
                      .ToListAsync();

  return Results.Ok(datos);
});

// Editar dato de subproyecto
datosSubproyectoGroup.MapPut("/{id}", async (StaticalyContext context, int id, DatosSubProyecto dato) =>
{
  var datoToUpdate = await context.DatosSubProyecto.FindAsync(id);
  if (datoToUpdate == null)
  {
    return Results.NotFound();
  }

  datoToUpdate.Nombre = dato.Nombre;
  datoToUpdate.Cantidad = dato.Cantidad;

  await context.SaveChangesAsync();

  return Results.NoContent();
}).Produces<DatosSubProyecto>();

// Eliminar dato de subproyecto
datosSubproyectoGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var dato = await context.DatosSubProyecto.FindAsync(id);
  if (dato == null)
  {
    return Results.NotFound();
  }

  context.DatosSubProyecto.Remove(dato);
  await context.SaveChangesAsync();

  return Results.NoContent();
});

#endregion

app.Run();
