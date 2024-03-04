using Staticaly.Server.Models;
using Staticaly.Server.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore;
using System.Web;

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

  context.Entry(equipoToUpdate).CurrentValues.SetValues(equipo);
  await context.SaveChangesAsync();

  return Results.StatusCode(204);
}).Produces<Equipos>();

// Crear equipo
EquipoGroup.MapPost("/", async (StaticalyContext context, Equipos equipo) =>
{
  context.Equipos.Add(equipo);
  await context.SaveChangesAsync();
  return Results.Created($"/equipos/{equipo.EquipoID}", equipo);
}).WithName("GetEquipos").Produces<Equipos>();

#endregion


app.Run();
