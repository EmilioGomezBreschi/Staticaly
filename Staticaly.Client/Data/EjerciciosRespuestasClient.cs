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

    }
}