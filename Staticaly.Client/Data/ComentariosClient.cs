using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;


namespace Staticaly.Client.Data
{
    public class ComentariosClient
    {
        private readonly HttpClient httpClient;

        public ComentariosClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        //Get Comentarios de Publicacion
        public async Task<Comentarios[]> GetComentariosAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Comentarios[]>($"comentarios/byPublicacion/{id}") ?? Array.Empty<Comentarios>();
        }

        //Get Comentario by Foro
        public async Task<List<Comentarios>> GetComentariosByForoAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<List<Comentarios>>($"comentarios/byForo/{id}") ?? new List<Comentarios>();
        }

        //Create Comentario
        public async Task<HttpResponseMessage> CreateComentarioAsync(Comentarios comentario)
        {
            return await httpClient.PostAsJsonAsync("comentarios", comentario);
        }

        //Edit Comentario
        public async Task<HttpResponseMessage> UpdateComentarioAsync(Comentarios comentario)
        {
            return await httpClient.PutAsJsonAsync($"comentarios/{comentario.ComentarioID}", comentario);
        }

        //Update Reportes
        public async Task<HttpResponseMessage> UpdateReportesAsync(int id, int reportes)
        {
            return await httpClient.PutAsJsonAsync($"comentarios/{id}/reportes", reportes);
        }

        //Delete Comentario
        public async Task<HttpResponseMessage> DeleteComentarioAsync(int id)
        {
            return await httpClient.DeleteAsync($"comentarios/{id}");
        }
    }
}