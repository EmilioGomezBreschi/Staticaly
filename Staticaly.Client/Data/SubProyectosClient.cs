using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staticaly.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Staticaly.Client.Data
{
  public class SubProyectosClient
  {
    private readonly HttpClient _httpClient;

    public SubProyectosClient(HttpClient httpClient)
    {
      _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    //Get SubProyectos por Proyecto
    public async Task<SubProyectos[]> GetSubProyectosAsync(int id)
    {
      return await _httpClient.GetFromJsonAsync<SubProyectos[]>($"subproyectos/byProyecto/{id}") ?? Array.Empty<SubProyectos>();
    }

    //Get SubProyecto por Id
    public async Task<SubProyectos> GetSubProyectoAsync(int id)
    {
      return await _httpClient.GetFromJsonAsync<SubProyectos>($"subproyectos/{id}") ?? new SubProyectos();
    }

    //Create SubProyecto
    public async Task<HttpResponseMessage> CreateSubProyectoAsync(SubProyectos subproyecto)
    {
      return await _httpClient.PostAsJsonAsync("subproyectos", subproyecto);
    }

    //Edit SubProyecto
    public async Task<HttpResponseMessage> EditSubProyectoAsync(SubProyectos subproyecto)
    {
      return await _httpClient.PutAsJsonAsync("subproyectos", subproyecto);
    }

    //Delete SubProyecto
    public async Task<HttpResponseMessage> DeleteSubProyectoAsync(int id)
    {
      return await _httpClient.DeleteAsync($"subproyectos/{id}");
    }

    //Cambiar Estado SubProyecto
    public async Task<HttpResponseMessage> CambiarEstadoSubProyectoAsync(int id, string editando, int UserId)
    {
      return await _httpClient.PutAsync($"subproyectos/editado/{id}/{editando}/{UserId}", null);
    }
  }
}