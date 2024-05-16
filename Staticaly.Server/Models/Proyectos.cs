using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Staticaly.Server.Models
{
  public class Proyectos
  {
    [Key]
    public int ProyectoID { get; set; }
    [Required]
    [ForeignKey("UsuarioID")]
    public int UsuarioID { get; set; }
    public User? Usuario { get; set; }
    [Required]
    [ForeignKey("EquipoID")]
    public int EquipoID { get; set; }
    public Equipos? Equipo { get; set; }
    [Required]
    [StringLength(255)]
    public string? Titulo { get; set; }
    [StringLength(255)]
    public string? Descripcion { get; set; }
  }
}