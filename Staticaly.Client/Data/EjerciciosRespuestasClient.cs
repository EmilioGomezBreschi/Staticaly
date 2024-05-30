using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class EjerciciosRespuestasClient
    {
      private readonly HttpClient _httpClient;

      public EjerciciosRespuestasClient(HttpClient httpClient)
      {
        _httpClient = httpClient;
      }

      //Crear Ejercicio Respuesta
      public async Task<HttpResponseMessage> CrearEjercicioRespuestaAsync(EjerciciosRespuestas ejercicioRespuesta)
      {
        return await _httpClient.PostAsJsonAsync("ejerciciosRespuestas", ejercicioRespuesta);
      }

      //Obtener Ejercicios Respuestas por Usuario
      public async Task<List<EjerciciosRespuestas>> ObtenerEjerciciosRespuestasPorUsuarioAsync(int usuarioID)
      {
        return await _httpClient.GetFromJsonAsync<List<EjerciciosRespuestas>>($"ejerciciosRespuestas/byUsuario/{usuarioID}") ?? new List<EjerciciosRespuestas>();
      }

      //Obtener Ejercicios Respuestas por pregunta
      public async Task<List<EjerciciosRespuestas>> ObtenerEjerciciosRespuestasPorPreguntaAsync(int preguntaID)
      {
        return await _httpClient.GetFromJsonAsync<List<EjerciciosRespuestas>>($"ejerciciosRespuestas/byPregunta/{preguntaID}") ?? new List<EjerciciosRespuestas>();
      }

      //Obtener Ejercicios Respuestas por Ejercicio
      public async Task<List<EjerciciosRespuestas>> ObtenerEjerciciosRespuestasPorEjercicioAsync(int ejercicioID)
      {
        return await _httpClient.GetFromJsonAsync<List<EjerciciosRespuestas>>($"ejerciciosRespuestas/byEjercicio/{ejercicioID}") ?? new List<EjerciciosRespuestas>();
      }

      //Obtener Ejercicios Respuestas por Usuario y Ejercicio
      public async Task<List<EjerciciosRespuestas>> ObtenerEjerciciosRespuestasPorUsuarioYEjercicioAsync(int usuarioID, int ejercicioID)
      {
        return await _httpClient.GetFromJsonAsync<List<EjerciciosRespuestas>>($"ejerciciosRespuestas/byUsuarioEjercicio/{usuarioID}/{ejercicioID}") ?? new List<EjerciciosRespuestas>();
      }

      //Obtener Ejercicios Respuestas por Usuario y Pregunta
      public async Task<List<EjerciciosRespuestas>> ObtenerEjerciciosRespuestasPorUsuarioYPreguntaAsync(int usuarioID, int preguntaID)
      {
        return await _httpClient.GetFromJsonAsync<List<EjerciciosRespuestas>>($"ejerciciosRespuestas/byUsuarioPregunta/{usuarioID}/{preguntaID}") ?? new List<EjerciciosRespuestas>();
      }

    }
}