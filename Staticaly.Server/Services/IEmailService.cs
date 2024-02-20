using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staticaly.Server.Models;

namespace Staticaly.Server.Services
{
  public interface IEmailService
  {
    void SendEmail(EmailDTO request);
  }
}