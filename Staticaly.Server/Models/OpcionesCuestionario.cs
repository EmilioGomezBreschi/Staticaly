using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Server.Models
{
    public class OpcionesCuestionario
    {
      [Key]
      public int OpcionID { get; set; }
      [Required]
      [ForeignKey("PreguntaID")]
      public int PreguntaID { get; set; }
      public Preguntas? Pregunta { get; set; }
      [Required]
      [StringLength(255)]
      public string? Opcion { get; set; }
    }
}