using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Staticaly.Server.Models
{
    public class StaticalyContext : DbContext
    {
      public DbSet<User> Usuarios { get; set; }
      public DbSet<Rol> Roles { get; set; }
      public StaticalyContext(DbContextOptions<StaticalyContext> options) : base(options) { }
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
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
      }
    }
}
