using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Staticaly.Server.Models
{
  public class StaticalyContext : DbContext
  {
    public DbSet<User> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Rango> Rangos { get; set; }
    public DbSet<Equipos> Equipos { get; set; }
    public DbSet<Permisos> Permisos { get; set; }
    public DbSet<TiposEquipos> TiposEquipos { get; set; }
    public DbSet<UsuariosEquipos> UsuariosEquipos { get; set; }

    public StaticalyContext(DbContextOptions<StaticalyContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      ApplyEntityConfigurations(modelBuilder);
      SeedData.Initialize(modelBuilder);
    }

    private void ApplyEntityConfigurations(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      ConfigureUser(modelBuilder);
      ConfigureEquipos(modelBuilder);
      ConfigureUsuariosEquipos(modelBuilder);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>()
          .HasOne(u => u.Rol)
          .WithMany()
          .HasForeignKey(u => u.RolID);

      modelBuilder.Entity<User>()
          .HasOne(u => u.Rango)
          .WithMany()
          .HasForeignKey(u => u.RangoID)
          .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureEquipos(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Equipos>()
          .HasOne(e => e.TipoEquipo)
          .WithMany()
          .HasForeignKey(e => e.TipoEquipoID);
    }

    private void ConfigureUsuariosEquipos(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UsuariosEquipos>()
          .HasOne(ue => ue.Usuario)
          .WithMany()
          .HasForeignKey(ue => ue.UsuarioID);

      modelBuilder.Entity<UsuariosEquipos>()
          .HasOne(ue => ue.Equipo)
          .WithMany()
          .HasForeignKey(ue => ue.EquipoID);

      modelBuilder.Entity<UsuariosEquipos>()
          .HasOne(ue => ue.Permiso)
          .WithMany()
          .HasForeignKey(ue => ue.PermisoID);
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
      modelBuilder.Entity<Permisos>().HasData(
          new Permisos
          {
            PermisoID = 1,
            Nombre = "Administrar"
          },
          new Permisos
          {
            PermisoID = 2,
            Nombre = "Visualizar"
          },
          new Permisos
          {
            PermisoID = 3,
            Nombre = "Editar"
          }
      );
      modelBuilder.Entity<TiposEquipos>().HasData(
          new TiposEquipos
          {
            TipoEquipoID = 1,
            Nombre = "Publico"
          },
          new TiposEquipos
          {
            TipoEquipoID = 2,
            Nombre = "Estudiante"
          },
          new TiposEquipos
          {
            TipoEquipoID = 3,
            Nombre = "Docente"
          }
      );
    }
  }
}
