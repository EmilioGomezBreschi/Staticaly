using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Staticaly.Server.Models
{
    public class StaticalyContext : DbContext
    {
      public DbSet<User> Usuarios { get; set; }
      public StaticalyContext(DbContextOptions<StaticalyContext> options) : base(options) { }
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      }
    }
}
