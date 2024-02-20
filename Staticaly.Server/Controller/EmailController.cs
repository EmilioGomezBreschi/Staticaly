using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Staticaly.Server.Services;
using Staticaly.Server.Models;

namespace Staticaly.Server.Controller
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmailController : ControllerBase
  {
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
      _emailService = emailService;
    }

    [HttpPost]
    public IActionResult SendEmail(EmailDTO request)
    {
      _emailService.SendEmail(request);
      return Ok();
    }
  }
}
