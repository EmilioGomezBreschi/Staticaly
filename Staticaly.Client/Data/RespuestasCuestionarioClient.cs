using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class RespuestasCuestionarioClient
    {
      private readonly HttpClient _httpClient;

      public RespuestasCuestionarioClient(HttpClient httpClient)
      {
        _httpClient = httpClient;
      }

      //Crear Respuesta Cuestionario
      public async Task<HttpResponseMessage> CrearRespuestaCuestionarioAsync(RespuestasCuestionarios respuestaCuestionario)
      {
        return await _httpClient.PostAsJsonAsync("respuestasCuestionario", respuestaCuestionario);
      }

      //Obtener Respuestas Cuestionario por Usuario
      public async Task<List<RespuestasCuestionarios>> ObtenerRespuestasCuestionarioPorUsuarioAsync(int usuarioID)
      {
        return await _httpClient.GetFromJsonAsync<List<RespuestasCuestionarios>>($"respuestasCuestionario/byUsuario/{usuarioID}") ?? new List<RespuestasCuestionarios>();
      }

      //Obtener Respuestas Cuestionario por Cuestionario
      public async Task<List<RespuestasCuestionarios>> ObtenerRespuestasCuestionarioPorCuestionarioAsync(int cuestionarioID)
      {
        return await _httpClient.GetFromJsonAsync<List<RespuestasCuestionarios>>($"respuestasCuestionario/byCuestionario/{cuestionarioID}") ?? new List<RespuestasCuestionarios>();
      }

      //Eliminar Respuesta Cuestionario
      public async Task<HttpResponseMessage> EliminarRespuestaCuestionarioAsync(int respuestaCuestionarioID)
      {
        return await _httpClient.DeleteAsync($"respuestasCuestionario/{respuestaCuestionarioID}");
      }
    }
}