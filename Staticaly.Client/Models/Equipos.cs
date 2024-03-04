using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class Equipos
  {
    [Key]
    public int EquipoID { get; set; }
    [Required]
    [StringLength(50)]
    public string? Nombre { get; set; }
    [StringLength(255)]
    public string? Descripcion { get; set; }
    public TiposEquipos? TipoEquipo { get; set; }
    [Required]
    [ForeignKey("TipoEquipoID")]
    public int TipoEquipoID { get; set; }
  }
}