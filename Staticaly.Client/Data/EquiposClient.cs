using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
    public class EquiposClient
    {
        private readonly HttpClient httpClient;

        public EquiposClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        // Get Todos los equipos
        public async Task<Equipos[]> GetEquiposAsync()
        {
            return await httpClient.GetFromJsonAsync<Equipos[]>("equipos") ?? Array.Empty<Equipos>();
        }

        // Get Un equipo por ID
        public async Task<Equipos?> GetEquipoAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Equipos>($"equipos/{id}");
        }

        // Crear un equipo
        public async Task<HttpResponseMessage> CreateEquipoAsync(Equipos equipo)
        {
            return await httpClient.PostAsJsonAsync("equipos", equipo);
        }

        // Actualizar un equipo
        public async Task<HttpResponseMessage> UpdateEquipoAsync(Equipos equipo)
        {
            return await httpClient.PutAsJsonAsync($"equipos/{equipo.EquipoID}", equipo);
        }

        // Borrar un equipo
        public async Task<HttpResponseMessage> DeleteEquipoAsync(int id)
        {
            return await httpClient.DeleteAsync($"equipos/{id}");
        }

        //Agregar un usuario a un equipo
        public async Task<HttpResponseMessage> AddUsuarioToEquipoAsync(UsuariosEquipos usuarioEquipo)
        {
            return await httpClient.PostAsJsonAsync("usuariosequipos", usuarioEquipo);
        }

        //Borrar un usuario de un equipo
        public async Task<HttpResponseMessage> DeleteUsuarioFromEquipoAsync(int id)
        {
            return await httpClient.DeleteAsync($"usuariosequipos/{id}");
        }

        // Get Todos los usuarios de un equipo
        public async Task<UsuariosEquipos[]> GetUsuariosFromEquipoAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<UsuariosEquipos[]>($"usuariosequipos/{id}") ?? Array.Empty<UsuariosEquipos>();
        }

        // Get Todos los equipos de un usuario
        public async Task<UsuariosEquipos[]> GetEquiposFromUsuarioAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<UsuariosEquipos[]>($"usuariosequipos/byUsuario/{id}") ?? Array.Empty<UsuariosEquipos>();
        }

        // Update permisos de un usuario en un equipo
        public async Task<HttpResponseMessage> UpdatePermisoUsuarioAsync(int id, int permisoID)
        {
            return await httpClient.PutAsync($"usuariosequipos/{id}/{permisoID}", null);
        }
    }
}