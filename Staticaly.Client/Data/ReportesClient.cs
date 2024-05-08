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
  public class ReportesClient
  {
    private readonly HttpClient httpClient;

    public ReportesClient(HttpClient httpClient)
    {
      this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    // Crear reporte
    public async Task<HttpResponseMessage> CreateReporteAsync(Reportes reporte)
    {
      return await httpClient.PostAsJsonAsync("reportes", reporte);
    }

    // Obtener reportes por Usuario
    public async Task<IEnumerable<Reportes>> GetReportesByUsuarioAsync(int usuarioID)
    {
      var result = await httpClient.GetFromJsonAsync<IEnumerable<Reportes>>($"reportes/byUsuario/{usuarioID}");
      return result ?? Enumerable.Empty<Reportes>();
    }

    // Obtener reportes por Comentario
    public async Task<IEnumerable<Reportes>> GetReportesByComentarioAsync(int comentarioID)
    {
      var result = await httpClient.GetFromJsonAsync<IEnumerable<Reportes>>($"reportes/byComentario/{comentarioID}");
      return result ?? Enumerable.Empty<Reportes>();
    }


    // Obtener reporte por Publicacion
    public async Task<IEnumerable<Reportes>> GetReportesByPublicacionAsync(int publicacionID)
    {
      var result = await httpClient.GetFromJsonAsync<IEnumerable<Reportes>>($"reportes/byPublicacion/{publicacionID}");
      return result ?? Enumerable.Empty<Reportes>();
    }

    // Obtener conteo de reportes por Grupo
    public async Task<int> GetReportesCountByGrupoAsync(int grupoID)
    {
      var response = await httpClient.GetAsync($"reportes/byGrupo/{grupoID}");

      if (response.IsSuccessStatusCode)
      {
        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);
        var rootElement = jsonDocument.RootElement;

        if (rootElement.TryGetProperty("reportesGenerales", out var reportesGeneralesElement) && reportesGeneralesElement.ValueKind == JsonValueKind.Number)
        {
          return reportesGeneralesElement.GetInt32();
        }
        else
        {
          throw new Exception("No se pudo obtener el conteo de reportes por grupo");
        }
      }
      else
      {
        // Manejo de errores: puedes lanzar una excepción, registrar un mensaje de error, etc.
        throw new Exception("No se pudo obtener el conteo de reportes por grupo");
      }
    }

    // Eliminar reporte por Publicacion y por Usuario
    public async Task<HttpResponseMessage> DeleteReporteByPublicacionAsync(int publicacionID, int usuarioID)
    {
      return await httpClient.DeleteAsync($"reportes/byPublicacion/{publicacionID}/{usuarioID}");
    }

    // Eliminar reporte por Comentario y por Usuario
    public async Task<HttpResponseMessage> DeleteReporteByComentarioAsync(int comentarioID, int usuarioID)
    {
      return await httpClient.DeleteAsync($"reportes/byComentario/{comentarioID}/{usuarioID}");
    }
  }
}