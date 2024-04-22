using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class EjerciciosClient
    {
      private readonly HttpClient _httpClient;

      public EjerciciosClient(HttpClient httpClient)
      {
        _httpClient = httpClient;
      }

      //Crear Ejercicio
      public async Task<HttpResponseMessage> CrearEjercicioAsync(Ejercicios ejercicio)
      {
        return await _httpClient.PostAsJsonAsync("ejercicios", ejercicio);
      }

      //Obtener Ejercicios por Equipo
      public async Task<List<Ejercicios>> ObtenerEjerciciosPorEquipoAsync(int equipoID)
      {
        return await _httpClient.GetFromJsonAsync<List<Ejercicios>>($"ejercicios/byEquipo/{equipoID}") ?? new List<Ejercicios>();
      }

      //Obtener Ejercicio por ID
      public async Task<Ejercicios> ObtenerEjercicioPorIDAsync(int ejercicioID)
      {
        return await _httpClient.GetFromJsonAsync<Ejercicios>($"ejercicios/{ejercicioID}") ?? new Ejercicios();
      }

      //Actualizar Ejercicio
      public async Task<HttpResponseMessage> ActualizarEjercicioAsync(Ejercicios ejercicio)
      {
        return await _httpClient.PutAsJsonAsync($"ejercicios/{ejercicio.EjerciciosID}", ejercicio);
      }

      //Eliminar Ejercicio
      public async Task<HttpResponseMessage> EliminarEjercicioAsync(int ejercicioID)
      {
        return await _httpClient.DeleteAsync($"ejercicios/{ejercicioID}");
      }

    }
}