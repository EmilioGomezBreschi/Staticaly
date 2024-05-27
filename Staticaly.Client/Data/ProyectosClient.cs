using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staticaly.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Staticaly.Client.Data
{
    public class ProyectosClient
    {
      private readonly HttpClient _httpClient;

      public ProyectosClient(HttpClient httpClient)
      {
          _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
      }

      //Get Proyectos por Equipo
      public async Task<Proyectos[]> GetProyectosAsync(int id)
      {
          return await _httpClient.GetFromJsonAsync<Proyectos[]>($"proyectos/byEquipo/{id}") ?? Array.Empty<Proyectos>();
      }

      //Get Proyecto por ID
      public async Task<Proyectos> GetProyectoAsync(int id)
      {
          return await _httpClient.GetFromJsonAsync<Proyectos>($"proyectos/{id}") ?? new Proyectos();
      }

      //Create Proyecto
      public async Task<HttpResponseMessage> CreateProyectoAsync(Proyectos proyecto)
      {
          return await _httpClient.PostAsJsonAsync("proyectos", proyecto);
      }

      //Edit Proyecto
      public async Task<HttpResponseMessage> EditProyectoAsync(Proyectos proyecto)
      {
          return await _httpClient.PutAsJsonAsync("proyectos", proyecto);
      }

      //Delete Proyecto
      public async Task<HttpResponseMessage> DeleteProyectoAsync(int id)
      {
          return await _httpClient.DeleteAsync($"proyectos/{id}");
      }
    }
}