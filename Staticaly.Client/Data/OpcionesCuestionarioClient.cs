using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class OpcionesCuestionarioClient
    {
        private readonly HttpClient httpClient;

        public OpcionesCuestionarioClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        //Get Opciones de Pregunta por pregunta
        public async Task<OpcionesCuestionario[]> GetOpcionesAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<OpcionesCuestionario[]>($"opcionesCuestionario/byPregunta/{id}") ?? Array.Empty<OpcionesCuestionario>();
        }

        //Create Opcion
        public async Task<HttpResponseMessage> CreateOpcionAsync(OpcionesCuestionario opcion)
        {
            return await httpClient.PostAsJsonAsync("opcionesCuestionario", opcion);
        }

        //Edit Opcion
        public async Task<HttpResponseMessage> UpdateOpcionAsync(OpcionesCuestionario opcion)
        {
            return await httpClient.PutAsJsonAsync($"opcionesCuestionario/{opcion.OpcionID}", opcion);
        }

        //Delete Opcion
        public async Task<HttpResponseMessage> DeleteOpcionAsync(int id)
        {
            return await httpClient.DeleteAsync($"opcionesCuestionario/{id}");
        }
    }
}