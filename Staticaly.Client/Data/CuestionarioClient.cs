using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
  public class CuestionarioClient
  {
    private readonly HttpClient _httpClient;

    public CuestionarioClient(HttpClient httpClient)
    {
      _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    //Crear cuestionario
    public async Task<HttpResponseMessage> CrearCuestionarioAsync(Cuestionarios cuestionario)
    {
      return await _httpClient.PostAsJsonAsync("cuestionarios", cuestionario);
    }

    //Obtener Cuestionario por Equipo
    public async Task<Cuestionarios[]> ObtenerCuestionarioPorEquipoAsync(int id)
    {
      return await _httpClient.GetFromJsonAsync<Cuestionarios[]>($"cuestionarios/byEquipo/{id}") ?? Array.Empty<Cuestionarios>();
    }

    //Obtener Cuestionario por Usuario
    public async Task<Cuestionarios[]> ObtenerCuestionarioPorUsuarioAsync(int id)
    {
      return await _httpClient.GetFromJsonAsync<Cuestionarios[]>($"cuestionarios/byUsuario/{id}") ?? Array.Empty<Cuestionarios>();
    }

    //Obtener Cuestionario por ID
    public async Task<Cuestionarios?> ObtenerCuestionarioPorIDAsync(int id)
    {
      try
      {
        return await _httpClient.GetFromJsonAsync<Cuestionarios>($"cuestionarios/{id}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error al obtener el cuestionario con ID {id}: {ex.Message}");
        return null;
      }
    }

    //Eliminar Cuestionario
    public async Task<HttpResponseMessage> EliminarCuestionarioAsync(int id)
    {
      return await _httpClient.DeleteAsync($"cuestionarios/{id}");
    }

    //Actualizar Estado Cuestionario
    public async Task<HttpResponseMessage> ActualizarEstadoCuestionarioAsync(int id, string estado)
    {
      return await _httpClient.PutAsync($"cuestionarios/publicado/{id}/{estado}", null);
    }

    //Actualizar respuestas de cuestionario
    public async Task<HttpResponseMessage> ActualizarRespuestasCuestionarioAsync(int cuestionarioid)
    {
      return await _httpClient.PutAsync($"cuestionarios/respuestas/{cuestionarioid}", null);
    }

    //Actualizar Cuestionario
    public async Task<HttpResponseMessage> ActualizarCuestionarioAsync(Cuestionarios cuestionario)
    {
      return await _httpClient.PutAsJsonAsync($"cuestionarios/{cuestionario.CuestionarioID}", cuestionario);
    }
  }
}