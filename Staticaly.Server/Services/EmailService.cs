using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staticaly.Server.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Staticaly.Server.Services
{
  public class EmailService : IEmailService
  {

    private readonly IConfiguration _config;
    public EmailService(IConfiguration config)
    {
      _config = config;
    }
    public void SendEmail(EmailDTO request)
    {
      var email = new MimeMessage();
      email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
      email.To.Add(MailboxAddress.Parse(request.Para));
      email.Subject = request.Asunto;
      email.Body = new TextPart(TextFormat.Html)
      {
        Text = request.Contenido
      };

      using var smtp = new SmtpClient();
      smtp.Connect(
        _config.GetSection("Email:Host").Value,
        Convert.ToInt32(_config.GetSection("Email:Port").Value),
        SecureSocketOptions.StartTls
      );
      smtp.Authenticate(
        _config.GetSection("Email:Username").Value,
        _config.GetSection("Email:Password").Value
      );
      smtp.Send(email);
      smtp.Disconnect(true);
    }
  }
}