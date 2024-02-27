using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Staticaly.Server.Models
{
  public class StaticalyContext : DbContext
  {
    public DbSet<User> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Rango> Rangos { get; set; }

    public StaticalyContext(DbContextOptions<StaticalyContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<User>()
          .HasOne(u => u.Rol)
          .WithMany()
          .HasForeignKey(u => u.RolID);

      modelBuilder.Entity<User>()
          .HasOne(u => u.Rango)
          .WithMany()
          .HasForeignKey(u => u.RangoID)
          .OnDelete(DeleteBehavior.Restrict);

      SeedData.Initialize(modelBuilder);
    }
  }

  public static class SeedData
  {
    public static void Initialize(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Rol>().HasData(
          new Rol
          {
            RolID = 1,
            RolName = "Admin"
          },
          new Rol
          {
            RolID = 2,
            RolName = "Estudiante"
          },
          new Rol
          {
            RolID = 3,
            RolName = "Docente"
          }
      );
      modelBuilder.Entity<Rango>().HasData(
          new Rango
          {
            RangoID = 1,
            NombreRango = "Nuevo",
            PuntosMin = 0,
            PuntosMax = 99
          },
          new Rango
          {
            RangoID = 2,
            NombreRango = "Viajero",
            PuntosMin = 100,
            PuntosMax = 5000
          },
          new Rango
          {
            RangoID = 3,
            NombreRango = "Explorador",
            PuntosMin = 5001,
            PuntosMax = 10000
          },
          new Rango
          {
            RangoID = 4,
            NombreRango = "Navegante",
            PuntosMin = 10001,
            PuntosMax = 15000
          },
          new Rango
          {
            RangoID = 5,
            NombreRango = "Analista",
            PuntosMin = 15001,
            PuntosMax = 20000
          },
          new Rango
          {
            RangoID = 6,
            NombreRango = "Estadistico",
            PuntosMin = 20001,
            PuntosMax = 25000
          },
          new Rango
          {
            RangoID = 7,
            NombreRango = "Coordinador",
            PuntosMin = 25001,
            PuntosMax = 30000
          },
          new Rango
          {
            RangoID = 8,
            NombreRango = "Arquitecto",
            PuntosMin = 30001,
            PuntosMax = 35000
          },
          new Rango
          {
            RangoID = 9,
            NombreRango = "Maestro",
            PuntosMin = 35001,
            PuntosMax = 40000
          },
          new Rango
          {
            RangoID = 10,
            NombreRango = "Guardian",
            PuntosMin = 40001,
            PuntosMax = 100000
          }
      );
    }
  }
}
