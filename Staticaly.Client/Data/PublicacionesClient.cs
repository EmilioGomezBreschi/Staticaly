using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;
using System.Text.Json;

namespace Staticaly.Client.Data
{
  public class PublicacionesClient
  {
    private readonly HttpClient httpClient;

    public PublicacionesClient(HttpClient httpClient)
    {
      this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    // Crear publicacion
    public async Task<HttpResponseMessage> CreatePublicacionAsync(Publicaciones publicacion)
    {
      return await httpClient.PostAsJsonAsync("publicaciones", publicacion);
    }

    // Obtener publicaciones de equipo
    public async Task<Publicaciones[]> GetPublicacionesAsync(int equipoID)
    {
      return await httpClient.GetFromJsonAsync<Publicaciones[]>($"publicaciones/byEquipo/{equipoID}") ?? Array.Empty<Publicaciones>();
    }

    // obtener publicacion de equipo por nombre
    public async Task<Publicaciones?> GetPublicacionAsync(int equipoID, string nombre)
    {
      return await httpClient.GetFromJsonAsync<Publicaciones>($"publicaciones/byEquipo/{equipoID}/{nombre}");
    }

    // Editar publicacion
    public async Task<HttpResponseMessage> EditPublicacionAsync(Publicaciones publicacion)
    {
      return await httpClient.PutAsJsonAsync($"publicaciones/{publicacion.PublicacionID}", publicacion);
    }

    //Actualizar calificacion de publicacion
    public async Task<HttpResponseMessage> UpdateCalificacionAsync(int publicacionID, float calificacion)
    {
      return await httpClient.PutAsJsonAsync($"publicaciones/{publicacionID}/calificacion", calificacion);
    }

    public async Task<RespuestaPaginada<Publicaciones>> ObtenerPublicaciones(int pagina, int porPagina, int equipoId)
    {
      // Llama a tu backend para obtener las publicaciones de la página actual
      var response = await httpClient.GetAsync($"publicaciones/byEquipo/{equipoId}?page={pagina}&pageSize={porPagina}");

      if (response.IsSuccessStatusCode)
      {
        var contenido = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RespuestaPaginada<Publicaciones>>(contenido, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new RespuestaPaginada<Publicaciones>(); // Return a default instance if deserialization fails
      }
      else
      {
        // Maneja el caso de error aquí, por ejemplo, registrando un mensaje de error
        // o devolviendo un objeto de respuesta vacío
        return new RespuestaPaginada<Publicaciones>(); // Return a default instance in case of error
      }
    }

  }
}