using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Staticaly.Client.Models;

namespace Staticaly.Client.Data
{
  public class EmailServiceClient
  {
    private readonly HttpClient httpClient;

    public EmailServiceClient(HttpClient httpClient)
    {
      this.httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> SendEmailAsync(EmailDTO email)
    {
      return await httpClient.PostAsJsonAsync("email", email);
    }

    public string HacerBodyVerificacion(string? nombre, string? apellido, string? verificationToken)
    {
      // Crear el cuerpo del correo electrónico de verificación
      return $@"<!DOCTYPE html>
                  <html lang='en'>
                  <head>
                      <meta charset='UTF-8'>
                      <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                      <title>Nuevo docente registrado - Staticaly</title>
                      <style>
                          body {{
                              font-family: Montserrat, sans-serif;
                              background-color: #000a23;
                              font-size: 1rem;
                              margin: 0;
                              padding: 0;
                              display: flex;
                              justify-content: center;
                              align-items: center;
                              height: 100vh;
                          }}
                          .container {{
                              max-width: 600px;
                              background-color: #557996;
                              border-radius: 10px;
                              padding: 20px;
                              box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                              text-align: center;
                          }}
                          h1 {{
                              color: #02253d;
                              margin-bottom: 20px;
                          }}
                          p {{
                              color: #d2e3f1;
                              margin-bottom: 10px;
                          }}
                          a {{
                            font-size: 1.2rem;
                              color: #007bff;
                              text-decoration: none;
                          }}
                          a:hover {{
                              text-decoration: underline;
                          }}
                          #token {{
                              font-size: 1.5rem;
                              font-weight: bold;
                          }}
                      </style>
                  </head>
                  <body>
                      <div class='container'>
                          <h1>¡Hola {nombre} {apellido}!</h1>
                          <p>Gracias por registrarte en Statica.ly. Por favor, haz clic en el siguiente enlace para verificar tu correo electrónico:</p>
                          <p><a href='http://localhost:5197/verificar'>Verificar correo</a></p>
                          <p>El código de verificación es: </p>
                          <p id='token'> {verificationToken}</p>
                      </div>
                  </body>
                  </html>";
    }

    public string HacerBodyDocente(string? nombre, string? apellido, string? institucion, int? semestre)
    {

      // Construye el cuerpo del correo electrónico
      return $@"<!DOCTYPE html>
          <html lang='en'>
          <head>
              <meta charset='UTF-8'>
              <meta name='viewport' content='width=device-width, initial-scale=1.0'>
              <title>Verificación de correo electrónico - Staticaly</title>
              <style>
                  body {{
                      font-family: Montserrat, sans-serif;
                      background-color: #000a23;
                      font-size: 1rem;
                      margin: 0;
                      padding: 0;
                      display: flex;
                      justify-content: center;
                      align-items: center;
                      height: 100vh;
                  }}
                  .container {{
                      max-width: 600px;
                      background-color: #557996;
                      border-radius: 10px;
                      padding: 20px;
                      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                      text-align: center;
                  }}
                  h2 {{
                      font-size: 24px;
                      color: #02253d;
                      margin-bottom: 20px;
                  }}
                  p {{
                      font-size: 16px;
                      margin-bottom: 10px;
                      color: #d2e3f1;
                      margin-bottom: 10px;
                  }}
                  strong {{
                      font-weight: bold;
                  }}
              </style>
          </head>
          <body>
            <div class='container'>
            <h2>Información del Docente</h2>
              <p><strong>Nombre:</strong> {nombre}</p>
              <p><strong>Apellido:</strong> {apellido}</p>
              <p><strong>Institución:</strong> {institucion}</p>
              <p><strong>Semestre:</strong> {semestre}</p>
            </div>
          </body>
          </html>";
    }

  }
}