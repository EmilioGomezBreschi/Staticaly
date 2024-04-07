using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class PreguntasClient
    {
        private readonly HttpClient httpClient;

        public PreguntasClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        //Get Preguntas de Cuestionario
        public async Task<Preguntas[]> GetPreguntasAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Preguntas[]>($"preguntas/byCuestionario/{id}") ?? Array.Empty<Preguntas>();
        }

        //Create Pregunta
        public async Task<HttpResponseMessage> CreatePreguntaAsync(Preguntas pregunta)
        {
            return await httpClient.PostAsJsonAsync("preguntas", pregunta);
        }

        //Edit Pregunta
        public async Task<HttpResponseMessage> UpdatePreguntaAsync(Preguntas pregunta)
        {
            return await httpClient.PutAsJsonAsync($"preguntas/{pregunta.PreguntaID}", pregunta);
        }

        //Delete Pregunta
        public async Task<HttpResponseMessage> DeletePreguntaAsync(int id)
        {
            return await httpClient.DeleteAsync($"preguntas/{id}");
        }
    }
}