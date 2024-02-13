using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Staticaly.Server.Models
{
  public class User
  {
    [Key]
    public int UsuarioID { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Nombre { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Apellido { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Email { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Password { get; set; }
    [Required]
    public int RolID { get; set; }
    [MaxLength(100)]
    public string? Imagen { get; set; }

  }
}