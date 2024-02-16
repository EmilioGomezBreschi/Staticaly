using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Prueba.Models;


namespace Prueba.Data
{
    public class UsuariosClient
    {
        private readonly HttpClient httpClient;

        public UsuariosClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<User[]?> GetUsuariosAsync()
        {
            return await httpClient.GetFromJsonAsync<User[]>("users");
        }

        public async Task<User?> GetUsuarioAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<User>($"users/{id}");
        }

        public async Task<HttpResponseMessage> CreateUsuarioAsync(User usuario)
        {
            return await httpClient.PostAsJsonAsync("users", usuario);
        }

        public async Task<HttpResponseMessage> UpdateUsuarioAsync(int id, User usuario)
        {
            return await httpClient.PutAsJsonAsync($"users/{id}", usuario);
        }

        public async Task<HttpResponseMessage> DeleteUsuarioAsync(int id)
        {
            return await httpClient.DeleteAsync($"users/{id}");
        }

        public async Task<User?> GetUsuarioByEmailAsync(string email)
        {
            return await httpClient.GetFromJsonAsync<User>($"users/byemail/{email}");
        }

        public async Task<string?> GetUsuarioEmailAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<string>($"users/byemail/{id}/email");
        }
    }
}