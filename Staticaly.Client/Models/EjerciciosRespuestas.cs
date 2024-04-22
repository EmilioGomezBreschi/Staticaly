using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class EjerciciosRespuestas
  {
    [Key]
    public int EjerciciosRespuestasID { get; set; }
    [ForeignKey("UsuarioID")]
    [Required]
    public int UsuarioID { get; set; }
    public User? User { get; set; }
    [ForeignKey("EjerciciosID")]
    [Required]
    public int EjercicioID { get; set; }
    [ForeignKey("EjerciciosPreguntasID")]
    [Required]
    public int EjercicioPreguntaID { get; set; }
    public EjerciciosPreguntas? EjerciciosPreguntas { get; set; }
    [Required]
    public string? Respuesta { get; set; }
  }
}