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

app.MapPost("/Email", (EmailDTO request, IEmailService emailService) =>
{
  emailService.SendEmail(request);
  return Results.Ok();
});

var userGroup = app.MapGroup("/users").WithParameterValidation();


#region Entry Points Users

// Get all users
userGroup.MapGet("/", async (StaticalyContext context) =>
    await context.Usuarios.Include(u => u.Rol)
                          .AsNoTracking()
                          .ToListAsync()
);

//Get user email
userGroup.MapGet("/byEmail/{id}/email", async (int id, StaticalyContext context) =>
{
  User? user = await context.Usuarios.FindAsync(id);
  if (user is null)
  {
    return Results.NotFound();
  }

  return Results.Ok(user.Email);
});

// Get user by email
userGroup.MapGet("/byEmail/{email}", async (string email, StaticalyContext context) =>
{
  User? user = await context.Usuarios.Include(u => u.Rol)
                                    .FirstOrDefaultAsync(user => user.Email == email);
  if (user is null)
  {
    return Results.NotFound();
  }
  return Results.Ok(user);
});

// Get user by id
userGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  User? user = await context.Usuarios.Include(u => u.Rol)
                                      .FirstOrDefaultAsync(user => user.UsuarioID == id);
  if (user is null)
  {
    return Results.NotFound();
  }
  return Results.Ok(user);
});

// Create user
userGroup.MapPost("/", async (StaticalyContext context, User user) =>
{
  context.Usuarios.Add(user);
  await context.SaveChangesAsync();
  return Results.CreatedAtRoute("GetUsers", new { id = user.UsuarioID }, user);
}).WithName("GetUsers");

// Update user
userGroup.MapPut("/{id}", async (StaticalyContext context, int id, User Updateduser) =>
{
  var RowsAffected = await context.Usuarios.Where(
      user => user.UsuarioID == id).ExecuteUpdateAsync(updates =>
      updates.SetProperty(user => user.Nombre, Updateduser.Nombre)
            .SetProperty(user => user.Apellido, Updateduser.Apellido)
            .SetProperty(user => user.Email, Updateduser.Email)
            .SetProperty(user => user.Password, Updateduser.Password)
            .SetProperty(user => user.RolID, Updateduser.RolID)
            .SetProperty(user => user.Imagen, Updateduser.Imagen)
  );
  return RowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

//Update user email verification
userGroup.MapPut("/verificar/{rawtoken}", async (string rawtoken, StaticalyContext context) =>
{
  try
  {
    string token = HttpUtility.UrlDecode(rawtoken);
    User? user = await context.Usuarios.FirstOrDefaultAsync(user => user.VerificationToken == token);
    if (user is null)
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

//Update user password
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



// Delete user
userGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var RowsAffected = await context.Usuarios.Where(
      user => user.UsuarioID == id).ExecuteDeleteAsync();
  return RowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

#endregion


app.Run();
