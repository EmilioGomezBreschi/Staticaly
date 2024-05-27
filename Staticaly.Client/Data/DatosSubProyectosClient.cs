using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staticaly.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Staticaly.Client.Data
{
    public class DatosSubProyectosClient
    {
      private readonly HttpClient _httpClient;

      public DatosSubProyectosClient(HttpClient httpClient)
      {
          _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
      }

      //Get DatosSubProyectos por SubProyecto
      public async Task<DatosSubProyecto[]> GetDatosSubProyectosAsync(int id)
      {
          return await _httpClient.GetFromJsonAsync<DatosSubProyecto[]>($"datossubproyecto/bySubproyecto/{id}") ?? Array.Empty<DatosSubProyecto>();
      }

      //Create DatosSubProyecto
      public async Task<HttpResponseMessage> CreateDatosSubProyectoAsync(DatosSubProyecto datossubproyecto)
      {
          return await _httpClient.PostAsJsonAsync("datossubproyecto", datossubproyecto);
      }

      //Edit DatosSubProyecto
      public async Task<HttpResponseMessage> EditDatosSubProyectoAsync(DatosSubProyecto datossubproyecto)
      {
          return await _httpClient.PutAsJsonAsync("datossubproyecto", datossubproyecto);
      }

      //Delete DatosSubProyecto
      public async Task<HttpResponseMessage> DeleteDatosSubProyectoAsync(int id)
      {
          return await _httpClient.DeleteAsync($"datossubproyecto/{id}");
      }
    }
}