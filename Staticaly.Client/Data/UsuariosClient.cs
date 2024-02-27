using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
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
      return await httpClient.GetFromJsonAsync<User>($"users/{id}?expand=Rol,Rango");
    }

    public async Task<HttpResponseMessage> CreateUsuarioAsync(User usuario)
    {
      string verificationToken = GenerateVerificationToken();
      usuario.VerificationToken = verificationToken;
      usuario.EmailVerified = false;

      return await httpClient.PostAsJsonAsync("users", usuario);
    }

    private string GenerateVerificationToken()
    {
      byte[] randomBytes = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomBytes);
      }
      return BitConverter.ToString(randomBytes).Replace("-", string.Empty);
    }

    public async Task<User?> GetUsuarioByEmailAsync(string? email)
    {
      try
      {
        return await httpClient.GetFromJsonAsync<User>($"users/byemail/{email}");
      }
      catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
      {
        return null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error al obtener el usuario por correo electrónico: " + ex.Message);
        throw;
      }
    }

    public async Task<string?> GetUsuarioEmailAsync(int id)
    {
      return await httpClient.GetFromJsonAsync<string>($"users/byemail/{id}/email");
    }

    public async Task<HttpResponseMessage> VerifyEmailAsync(string? token)
    {
      return await httpClient.PutAsync($"users/verificar/{token}", null);
    }

    public async Task<HttpResponseMessage> PasswordResetAsync(string? password, int id)
    {
      return await httpClient.PutAsync($"users/cambiarcontrasena/{id}/{password}", null);
    }
  }
}
