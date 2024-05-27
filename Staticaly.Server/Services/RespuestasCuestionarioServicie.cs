using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staticaly.Server.Models;

namespace Staticaly.Server.Services
{
  public class RespuestasCuestionarioServicie : IRespuestasCuestionarioService
  {

    private HttpClient _httpClient;

    public RespuestasCuestionarioServicie(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }
    public async Task<List<RespuestasCuestionario>> Lista(int id)
    {
      var RespuestasCuestionario = await _httpClient.GetFromJsonAsync<List<RespuestasCuestionario>>($"http://localhost:5233/respuestascuestionario/byCuestionario/{id}");
      return RespuestasCuestionario;
    }
  }
}