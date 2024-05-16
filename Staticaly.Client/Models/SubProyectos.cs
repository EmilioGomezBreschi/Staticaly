using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class SubProyectos
  {
    [Key]
    public int SubproyectoID { get; set; }
    [Required]
    [ForeignKey("ProyectoID")]
    public int ProyectoID { get; set; }
    public Proyectos? Proyecto { get; set; }
    [Required]
    [StringLength(255)]
    public string? Titulo { get; set; }
    [StringLength(255)]
    public string? Descripcion { get; set; }
    public bool? Editando { get; set; }
    [ForeignKey("UsuarioID")]
    public int UsuarioID { get; set; }
    public User? Usuario { get; set; }

  }
}