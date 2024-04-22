using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class EjerciciosPreguntasClient
    {
      private readonly HttpClient _httpClient;

      public EjerciciosPreguntasClient(HttpClient httpClient)
      {
        _httpClient = httpClient;
      }

      //Crear Ejercicio Pregunta
      public async Task<HttpResponseMessage> CrearEjercicioPreguntaAsync(EjerciciosPreguntas ejercicioPregunta)
      {
        return await _httpClient.PostAsJsonAsync("ejerciciospreguntas", ejercicioPregunta);
      }

      //Obtener Ejercicios Preguntas por Ejercicio
      public async Task<List<EjerciciosPreguntas>> ObtenerEjerciciosPreguntasPorEjercicioAsync(int ejercicioID)
      {
        return await _httpClient.GetFromJsonAsync<List<EjerciciosPreguntas>>($"ejerciciospreguntas/byEjercicio/{ejercicioID}") ?? new List<EjerciciosPreguntas>();
      }

      //Editar Ejercicio Pregunta
      public async Task<HttpResponseMessage> EditarEjercicioPreguntaAsync(EjerciciosPreguntas ejercicioPregunta)
      {
        return await _httpClient.PutAsJsonAsync($"ejerciciospreguntas/{ejercicioPregunta.EjerciciosPreguntasID}", ejercicioPregunta);
      }

      //Eliminar Ejercicio Pregunta
      public async Task<HttpResponseMessage> EliminarEjercicioPreguntaAsync(int ejercicioPreguntaID)
      {
        return await _httpClient.DeleteAsync($"ejerciciospreguntas/{ejercicioPreguntaID}");
      }
    }
}