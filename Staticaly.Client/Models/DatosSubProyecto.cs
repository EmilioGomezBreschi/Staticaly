using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class DatosSubProyecto
  {
    [Key]
    public int DatosSubProyectoID { get; set; }
    [Required]
    [ForeignKey("SubproyectoID")]
    public int SubproyectoID { get; set; }
    public SubProyectos? Subproyecto { get; set; }
    [Required]
    [StringLength(255)]
    public string? Nombre { get; set; }
    public int? Cantidad { get; set; }
  }
}