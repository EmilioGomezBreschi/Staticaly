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
    public DbSet<Publicaciones> Publicaciones { get; set; }
    public DbSet<Comentarios> Comentarios { get; set; }
    public DbSet<Cuestionarios> Cuestionarios { get; set; }
    public DbSet<Preguntas> Preguntas { get; set; }
    public DbSet<OpcionesCuestionario> OpcionesCuestionario { get; set; }
    public DbSet<Calificaciones> Calificaciones { get; set; }
    public DbSet<RespuestasCuestionario> RespuestasCuestionarios { get; set; }
    public DbSet<EjerciciosRespuestas> EjerciciosRespuestas { get; set; }
    public DbSet<EjerciciosPreguntas> EjerciciosPreguntas { get; set; }
    public DbSet<Ejercicios> Ejercicios { get; set; }
    public DbSet<Reportes> Reportes { get; set; }


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
      ConfigurarPublicaciones(modelBuilder);
      ConfigurarComentarios(modelBuilder);
      ConfigureCuestionarios(modelBuilder);
      ConfigurePreguntas(modelBuilder);
      ConfigureOpcionesCuestionario(modelBuilder);
      ConfigureCalificaciones(modelBuilder);
      ConfigureRespuestasCuestionario(modelBuilder);
      ConfigureEjerciciosRespuestas(modelBuilder);
      ConfigureEjerciciosPreguntas(modelBuilder);
      ConfigureEjercicios(modelBuilder);
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
    private void ConfigurarPublicaciones(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Publicaciones>()
          .HasOne(p => p.Usuario)
          .WithMany()
          .HasForeignKey(p => p.UsuarioID);
    }

    private void ConfigurarComentarios(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Comentarios>()
          .HasOne(c => c.Publicacion)
          .WithMany()
          .HasForeignKey(c => c.PublicacionID)
          .OnDelete(DeleteBehavior.Cascade); // Eliminar en cascada si se elimina la publicación

      modelBuilder.Entity<Comentarios>()
          .HasOne(c => c.Usuario)
          .WithMany()
          .HasForeignKey(c => c.UsuarioID)
          .OnDelete(DeleteBehavior.Restrict); // No realizar acción en cascada si se elimina el usuario
    }

    private void ConfigureCuestionarios(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Cuestionarios>()
          .HasOne(c => c.Equipo)
          .WithMany()
          .HasForeignKey(c => c.EquipoID);

      modelBuilder.Entity<Cuestionarios>()
          .HasOne(c => c.Usuario)
          .WithMany()
          .HasForeignKey(c => c.UsuarioID);
    }

    private void ConfigurePreguntas(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Preguntas>()
          .HasOne(p => p.Cuestionario)
          .WithMany()
          .HasForeignKey(p => p.CuestionarioID);
    }

    private void ConfigureOpcionesCuestionario(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<OpcionesCuestionario>()
          .HasOne(o => o.Pregunta)
          .WithMany()
          .HasForeignKey(o => o.PreguntaID);
    }

    private void ConfigureCalificaciones(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Calificaciones>()
          .HasOne(c => c.Usuario)
          .WithMany()
          .HasForeignKey(c => c.UsuarioID)
          .OnDelete(DeleteBehavior.Restrict); // No realizar acción en cascada si se elimina el usuario

      modelBuilder.Entity<Calificaciones>()
          .HasOne(c => c.Comentario)
          .WithMany()
          .HasForeignKey(c => c.ComentarioID)
          .OnDelete(DeleteBehavior.Cascade); // Eliminar en cascada si se elimina el comentario
    }

    private void ConfigureRespuestasCuestionario(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<RespuestasCuestionario>()
          .HasOne(rc => rc.User)
          .WithMany()
          .HasForeignKey(rc => rc.UsuarioID)
          .OnDelete(DeleteBehavior.Restrict); // No realizar acción en cascada si se elimina el usuario
    }

    private void ConfigureEjerciciosRespuestas(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<EjerciciosRespuestas>()
          .HasOne(er => er.EjerciciosPreguntas)
          .WithMany()
          .HasForeignKey(er => er.EjercicioPreguntaID)
          .OnDelete(DeleteBehavior.Cascade); // Eliminar en cascada si se elimina la pregunta

      modelBuilder.Entity<EjerciciosRespuestas>()
          .HasOne(er => er.User)
          .WithMany()
          .HasForeignKey(er => er.UsuarioID)
          .OnDelete(DeleteBehavior.Restrict); // No realizar acción en cascada si se elimina el usuario
    }

    private void ConfigureEjerciciosPreguntas(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<EjerciciosPreguntas>()
          .HasOne(ep => ep.Ejercicios)
          .WithMany()
          .HasForeignKey(ep => ep.EjercicioID)
          .OnDelete(DeleteBehavior.Cascade); // Eliminar en cascada si se elimina el ejercicio
    }

    private void ConfigureEjercicios(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Ejercicios>()
          .HasOne(e => e.User)
          .WithMany()
          .HasForeignKey(e => e.UsuarioID)
          .OnDelete(DeleteBehavior.Restrict); // No realizar acción en cascada si se elimina el usuario
    }

    private void ConfigureReportes(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Reportes>()
          .HasOne(r => r.User)
          .WithMany()
          .HasForeignKey(r => r.UsuarioID)
          .OnDelete(DeleteBehavior.Restrict); // No realizar acción en cascada si se elimina el usuario

      modelBuilder.Entity<Reportes>()
          .HasOne(r => r.Comentarios)
          .WithMany()
          .HasForeignKey(r => r.ComentarioID)
          .OnDelete(DeleteBehavior.Cascade); // Eliminar en cascada si se elimina el comentario

      modelBuilder.Entity<Reportes>()
          .HasOne(r => r.publicaciones)
          .WithMany()
          .HasForeignKey(r => r.PublicacionID)
          .OnDelete(DeleteBehavior.Cascade); // Eliminar en cascada si se elimina la publicación
    }
  }


  public static class SeedData
  {
    public static void Initialize(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().HasData(
          new User
          {
            UsuarioID = 1,
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "staticaly.services@gmail.com",
            Password = "$2a$12$wM8vSxJF5IO27LsmTeheheB.durdm4GUJp4RD9pLon4/fYTlR6mbS",
            RolID = 1,
            RangoID = 10,
            Puntos = 100000,
            EmailVerified = true,
            Imagen = null,
            VerificationToken = null
          });

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
      modelBuilder.Entity<Equipos>().HasData(
          new Equipos
          {
            EquipoID = 1,
            Nombre = "Foro Publico",
            TipoEquipoID = 1
          }
      );

      modelBuilder.Entity<Ejercicios>().HasData(
        new Ejercicios
        {
          EjerciciosID = 1,
          UsuarioID = 1,
          EquipoID = 1,
          Titulo = "Ejercicios Publicos",
          Descripcion = "En base a estos ejercicios sera el rango que tengas dentro de la plataforma, hay un total de 100 ejercicios los cuales van a ir incrementando su dificultad conforme vayas avanzando, por cada respuesta incorrecta se restaran puntos y por cada segundo que pase se restaran puntos, por lo que es importante que contestes lo mas rapido posible y de manera correcta.",
          Publicado = true,
          FechaCierre = null,
          Timer = 1000,
          PuntosAquitar = 100,
          permitirMasDeUnaRespuesta = true,
          PuntosMin = 100,
        }
      );

      modelBuilder.Entity<EjerciciosPreguntas>().HasData(
        new EjerciciosPreguntas
        {
          EjerciciosPreguntasID = 1,
          EjercicioID = 1,
          Pregunta = "Media (Promedio) Datos: 15, 20, 25, 30, 35.",
          Respuesta = "25",
          Puntos = 1000
        },
        new EjerciciosPreguntas
        {
          EjerciciosPreguntasID = 2,
          EjercicioID = 1,
          Pregunta = "Mediana Datos: 8, 4, 6, 12, 10",
          Respuesta = "8",
          Puntos = 1000
        },
        new EjerciciosPreguntas
        {
          EjerciciosPreguntasID = 3,
          EjercicioID = 1,
          Pregunta = "Moda Datos: 7, 4, 7, 9, 2, 4.",
          Respuesta = "7",
          Puntos = 1000
        },
        new EjerciciosPreguntas
        {
          EjerciciosPreguntasID = 4,
          EjercicioID = 1,
          Pregunta = "Varianza Datos: 5, 10, 15, 20, 25. ",
          Respuesta = "50",
          Puntos = 1000
        },
        new EjerciciosPreguntas
        {
          EjerciciosPreguntasID = 5,
          EjercicioID = 1,
          Pregunta = "5.	Desviación Estándar Datos: 9, 12, 15, 18, 21",
          Respuesta = "7.07",
          Puntos = 1000
        }
      );
    }
  }
}
