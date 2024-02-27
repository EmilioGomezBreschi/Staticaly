using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Server.Models
{
  public class Rango
  {
    [Key]
    public int RangoID { get; set; }
    [Required]
    [MaxLength(11)]
    public string? NombreRango { get; set; }
    [Required]
    public int PuntosMin { get; set; }
    [Required]
    public int PuntosMax { get; set; }

  }
}