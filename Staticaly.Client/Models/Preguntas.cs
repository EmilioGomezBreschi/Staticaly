using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class Preguntas
  {
    [Key]
    public int PreguntaID { get; set; }

    [Required]
    [ForeignKey("CuestionarioID")]
    public int CuestionarioID { get; set; }

    public Cuestionarios? Cuestionario { get; set; }

    [Required]
    [StringLength(255)]
    public string? Pregunta { get; set; }

    // Lista de respuestas asociadas a esta pregunta
    public List<OpcionesCuestionario> Opciones { get; set; } = new List<OpcionesCuestionario>();
    // Propiedad para mantener las opciones originales
    public List<string> OpcionesOriginales { get; set; } = new List<string>();
  }
}
