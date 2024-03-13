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

    public string? HacerBodyVerificacion(string? nombre, string? apellido, string? verificationToken)
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
                              font-size: 24px;
                              color: #02253d;
                              margin-bottom: 20px;
                          }}
                          p {{
                              font-size: 16px;
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

    public string? HacerBodyDocente(string? nombre, string? apellido, string? correo, string? institucion, int semestre)
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
                  a {{
                      font-size: 1.2rem;
                      color: #007bff;
                      text-decoration: none;
                  }}
                  a:hover {{
                      text-decoration: underline;
                  }}
              </style>
          </head>
          <body>
            <div class='container'>
            <h2>Información del Docente</h2>
              <p><strong>Nombre:</strong> {nombre}</p>
              <p><strong>Apellido:</strong> {apellido}</p>
              <p><strong>Correo:</strong> {correo}</p>
              <p><strong>Institución:</strong> {institucion}</p>
              <p><strong>Semestre:</strong> {semestre}</p>
              <a href='http://localhost:5197/verificarDocente'>Verificar Informacion Docente</a>
            </div>
          </body>
          </html>";
    }

    public string? HacerBodyRecuperacion(string? nombre, string? apellido, int ID)
    {
      return $@"<!DOCTYPE html>
            <html lang='en'>
              <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Cambio de contraseña - Staticaly</title>
                <style>
                  body {{
                    font-family: Montserrat, sans-serif;
                    background-color: #000a23;
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
                    font-size: 24px;
                    color: #02253d;
                    margin-bottom: 20px;
                  }}
                  p {{
                    font-size: 16px;
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
                  .logo {{
                    width: 100px;
                    height: auto;
                    margin-bottom: 20px;
                  }}
                </style>
              </head>
              <body>
                <div class='container'>
                  <h1>¡Hola {nombre} {apellido}!</h1>
                  <p>Escuchamos que olvidaste tu contraseña en Statica.ly. Por favor, haz clic en el siguiente enlace para cambiar tu contraseña:</p>
                  <p><a href='http://localhost:5197/cambiarcontrasena/{ID}'>Aquí</a></p>
                </div>
              </body>
            </html>";
    }

    public string? HacerBodyAceptacion()
    {
      return $@"<!DOCTYPE html>
            <html lang='en'>
              <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Ahora eres un Usuario Docente - Staticaly</title>
                <style>
                  body {{
                    font-family: Montserrat, sans-serif;
                    background-color: #000a23;
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
                    font-size: 24px;
                    color: #02253d;
                    margin-bottom: 20px;
                  }}
                  p {{
                    font-size: 16px;
                    color: #d2e3f1;
                    margin-bottom: 10px;
                  }}
                </style>
              </head>
              <body>
                <div class='container'>
                  <h1>¡Bienvenido a Statica.ly Docente!</h1>
                  <p>A partir de hoy podras disfrutar de todos loes beneficios de docente como crear grupos de hasta 50 personas, crear ejercicios personalizados y mucho mas.</p>
                </div>
              </body>
            </html>";
    }

    public string? HacerBodyRechazo(string? Asunto)
    {
      return $@"<!DOCTYPE html>
            <html lang='en'>
              <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Rechazo de solicitud - Staticaly</title>
                <style>
                  body {{
                    font-family: Montserrat, sans-serif;
                    background-color: #000a23;
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
                  #rechazo{{
                    font-size: 1.5rem;
                    font-weight: bold;
                    }}
                  a{{
                    font-size: 1.2rem;
                    color: #007bff;
                    text-decoration: none;
                  }}
                  a:hover{{
                    text-decoration: underline;
                  }}
                </style>
              </head>
              <body>
                <div class='container'>
                  <h1>¡Lo sentimos!</h1>
                  <p>Tu solicitud para ser docente en Statica.ly ha sido rechazada por:</p>
                  <p id='Rechazo'>{Asunto}</p>
                  <p>Si deseas mandar otra foto ingresa a este link:</p>
                  <p><a href='http://localhost:5197/ActualizarFotoDocente'>Mandar otra foto</a></p>
                </div>
              </body>
            </html>";
    }

    public string? HacerBodyInvitacionEquipo(string? nombreinvitado, string? apellidoinvitado, string? nombreinvitador, string? apellidoInvitador, string? nombreEquipo, int idequipo, int idinvitado)
    {
      return $@"<!DOCTYPE html>
            <html lang='en'>
              <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Invitación a un equipo - Staticaly</title>
                <style>
                  body {{
                    font-family: Montserrat, sans-serif;
                    background-color: #000a23;
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
                    font-size: 24px;
                    color: #02253d;
                    margin-bottom: 20px;
                  }}
                  p {{
                    font-size: 16px;
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
                  <h1>¡Hola {nombreinvitado} {apellidoinvitado}!</h1>
                  <p> {nombreinvitador} {apellidoInvitador} te ha invitado a unirte a su equipo {nombreEquipo}.</p>
                  <p>Para unirte a su equipo, por favor haz clic en el siguiente enlace:</p>
                  <p><a href='http://localhost:5197/UnirEquipo/{idinvitado}/{idequipo}'>Unirme al equipo</a></p>
                </div>
              </body>
            </html>";
    }
  }
}