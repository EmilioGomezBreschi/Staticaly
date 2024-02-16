using Staticaly.Server.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(
    builder =>
    {
      builder.WithOrigins("http://localhost:5289")
            .AllowAnyHeader()
            .AllowAnyMethod();
    }));

var connectionString = builder.Configuration.GetConnectionString("StaticalyContext");
builder.Services.AddSqlServer<StaticalyContext>(connectionString);

var app = builder.Build();
app.UseCors();

var userGroup = app.MapGroup("/users").WithParameterValidation();

#region Entry Points Users

// Get all users
userGroup.MapGet("/", async (StaticalyContext context) =>
    await context.Usuarios.AsNoTracking().ToListAsync()
);

//Get user email
userGroup.MapGet("/ByEmail/{id}/email", async (int id, StaticalyContext context) =>
{
  User? user = await context.Usuarios.FindAsync(id);
  if (user is null)
  {
    return Results.NotFound();
  }

  return Results.Ok(user.Email);
});

// Get user by email
userGroup.MapGet("/ByEmail/{email}", async (string email, StaticalyContext context) =>
{
  User? user = await context.Usuarios.FirstOrDefaultAsync(user => user.Email == email);
  if (user is null)
  {
    return Results.NotFound();
  }
  return Results.Ok(user);
});

// Get user by id
userGroup.MapGet("/{id}", async (StaticalyContext context, int id) =>
{
  User? user = await context.Usuarios.FindAsync(id);
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

// Delete user
userGroup.MapDelete("/{id}", async (StaticalyContext context, int id) =>
{
  var RowsAffected = await context.Usuarios.Where(
      user => user.UsuarioID == id).ExecuteDeleteAsync();
  return RowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

#endregion

app.Run();
