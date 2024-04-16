using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Staticaly.Client.Models;
using System.Net;

namespace Staticaly.Client.Data
{
  public class CalificacionesClient
  {
    private readonly HttpClient httpClient;

    public CalificacionesClient(HttpClient httpClient)
    {
      this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task CrearOActualizarCalificacionAsync(int comentarioID, int usuarioID, float calificacion)
    {
      // Verificar si el usuario ya ha calificado
      var existingCalificacion = await GetCalificacionAsync(comentarioID, usuarioID);

      if (existingCalificacion != null)
      {
        // Si el usuario ya ha calificado, actualizar la calificación existente
        existingCalificacion.Calificacion = calificacion;
        await UpdateCalificacionAsync(existingCalificacion);
      }
      else
      {
        // Si el usuario no ha calificado, crear una nueva calificación
        var nuevaCalificacion = new Calificaciones
        {
          ComentarioID = comentarioID,
          UsuarioID = usuarioID,
          Calificacion = calificacion
        };
        await CreateCalificacionAsync(nuevaCalificacion);
      }
    }

    private async Task<Calificaciones?> GetCalificacionAsync(int comentarioID, int usuarioID)
    {
      var response = await httpClient.GetAsync($"calificaciones/byComentario/{comentarioID}/{usuarioID}");

      if (response.IsSuccessStatusCode)
      {
        return await response.Content.ReadFromJsonAsync<Calificaciones>();
      }
      else if (response.StatusCode == HttpStatusCode.NotFound)
      {
        // No se encontró la calificación para el usuario y el comentario específicos
        return null;
      }
      else
      {
        // Manejar otros códigos de estado de respuesta según sea necesario
        throw new Exception($"Error al obtener la calificación. Código de estado: {response.StatusCode}");
      }
    }

    private async Task<HttpResponseMessage> CreateCalificacionAsync(Calificaciones calificacion)
    {
      return await httpClient.PostAsJsonAsync("calificaciones", calificacion);
    }

    private async Task<HttpResponseMessage> UpdateCalificacionAsync(Calificaciones calificacion)
    {
      return await httpClient.PutAsJsonAsync($"calificaciones/{calificacion.ComentarioID}/{calificacion.UsuarioID}", calificacion);
    }

  }
}
