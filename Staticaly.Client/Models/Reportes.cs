using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class Reportes
  {
    [Key]
    public int ReportesID { get; set; }
    [Required]
    [ForeignKey("UsuarioID")]
    public int UsuarioID { get; set; }
    public User? User { get; set; }
    [Required]
    [ForeignKey("ComentarioID")]
    public int ComentarioID { get; set; }
    public Comentarios? Comentarios { get; set; }
    [Required]
    [ForeignKey("PublicacionID")]
    public int PublicacionID { get; set; }
    public Publicaciones? publicaciones { get; set; }
  }
}