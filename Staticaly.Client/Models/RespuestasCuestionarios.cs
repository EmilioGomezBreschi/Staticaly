using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class RespuestasCuestionarios
  {
    [Key]
    public int RespuestasCuestionarioID { get; set; }
    [ForeignKey("CuestionarioID")]
    [Required]
    public int CuestionarioID { get; set; }
    [ForeignKey("PreguntaID")]
    [Required]
    public int PreguntaID { get; set; }
    [Required]
    public int RespuestaID { get; set; } // ID de la opción seleccionada
    [ForeignKey("UsuarioID")]
    [Required]
    public int UsuarioID { get; set; }
    public User? User { get; set; }
  }
}