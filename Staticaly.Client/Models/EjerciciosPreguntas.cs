using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class EjerciciosPreguntas
  {
    [Key]
    public int EjerciciosPreguntasID { get; set; }
    [ForeignKey("EjerciciosID")]
    [Required]
    public int EjercicioID { get; set; }
    public Ejercicios? Ejercicios { get; set; }
    [Required]
    public string? Pregunta { get; set; }
    public byte[]? Imagen { get; set; }
    [Required]
    public string? Respuesta { get; set; }
    public int Puntos { get; set; }
  }
}