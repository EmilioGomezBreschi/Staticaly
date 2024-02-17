using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public interface IEmailService
{
  Task SendEmailAsync(string email, string subject, string body);
}

public class SmtpEmailService : IEmailService
{
  private readonly string smtpServer;
  private readonly int smtpPort;
  private readonly string smtpUsername;
  private readonly string smtpPassword;

  public SmtpEmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
  {
    this.smtpServer = smtpServer;
    this.smtpPort = smtpPort;
    this.smtpUsername = smtpUsername;
    this.smtpPassword = smtpPassword;
  }

  public async Task SendEmailAsync(string email, string subject, string body)
  {
    try
    {
      using (var client = new SmtpClient(smtpServer, smtpPort))
      {
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        client.EnableSsl = true;

        var message = new MailMessage();
        message.From = new MailAddress(smtpUsername);
        message.To.Add(email);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        await client.SendMailAsync(message);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error al enviar correo electrónico: {ex.Message}");
    }
  }
}