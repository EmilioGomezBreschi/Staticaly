using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staticaly.Client.Models
{
    public class RespuestaPaginada<T>
    {
    public List<T>? Publicaciones { get; set; }
    public int TotalPaginas { get; set; }
  }
}