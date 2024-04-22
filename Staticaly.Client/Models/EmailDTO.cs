using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staticaly.Client.Models
{
  public class EmailDTO
  {
    public string Para { get; set; } = string.Empty;
    public string Asunto { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
  }
}