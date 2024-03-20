using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
  public class UsuariosEquipos
  {
    [Key]
    public int UsuarioEquipoID { get; set; }
    public User? Usuario { get; set; }
    [Required]
    [ForeignKey("UsuarioID")]
    public int UsuarioID { get; set; }
    public Equipos? Equipo { get; set; }
    [Required]
    [ForeignKey("EquipoID")]
    public int EquipoID { get; set; }
    public Permisos? Permiso { get; set; }
    [Required]
    [ForeignKey("PermisoID")]
    public int PermisoID { get; set; }
    [Required]
    public DateTime FechaUnir { get; set; }
  }
}