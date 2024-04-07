using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
  }
}