using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Json;
using Staticaly.Client.Models;
using System.Net;
using System.Text.Json;


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
      return await httpClient.GetFromJsonAsync<User>($"users/{id}?include=rol");
    }

    public async Task<HttpResponseMessage> CreateUsuarioAsync(User usuario)
    {
      string verificationToken = GenerateVerificationToken(usuario);
      usuario.VerificationToken = verificationToken;
      usuario.EmailVerified = false;

      return await httpClient.PostAsJsonAsync("users", usuario);
    }

    private string GenerateVerificationToken(User user)
    {
      string tokenContent = $"{user.Nombre}-{user.Apellido}-{user.Email}-{DateTime.UtcNow}";

      using (var sha256 = SHA256.Create())
      {
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(tokenContent));
        string hashedToken = BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        return hashedToken;
      }
    }

    public async Task<HttpResponseMessage> UpdateUsuarioAsync(int id, User usuario)
    {
      return await httpClient.PutAsJsonAsync($"users/{id}", usuario);
    }

    public async Task<HttpResponseMessage> DeleteUsuarioAsync(int id)
    {
      return await httpClient.DeleteAsync($"users/{id}");
    }

    public async Task<User?> GetUsuarioByEmailAsync(string? email)
    {
      try
      {
        return await httpClient.GetFromJsonAsync<User>($"users/byemail/{email}");
      }
      catch (HttpRequestException ex)
      {
        if (ex.StatusCode == HttpStatusCode.NotFound)
        {
          return null;
        }
        else
        {
          Console.WriteLine("Error al obtener el usuario por correo electrónico: " + ex.Message);
          throw;
        }
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
  }
}