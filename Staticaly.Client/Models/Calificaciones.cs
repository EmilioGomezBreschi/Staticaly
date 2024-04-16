using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class Calificaciones
  {
    [Key]
    public int CalificacionID { get; set; }

    [Required]
    [ForeignKey("ComentarioID")] // Corregido
    public int ComentarioID { get; set; }
    public Comentarios? Comentario { get; set; }

    [Required]
    [ForeignKey("UsuarioID")] // Corregido
    public int UsuarioID { get; set; }
    public User? Usuario { get; set; }

    [Required]
    public float? Calificacion { get; set; }
  }
}
