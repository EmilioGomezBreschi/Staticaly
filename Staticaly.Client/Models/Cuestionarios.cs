using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Client.Models
{
    public class Cuestionarios
    {
    [Key]
    public int CuestionarioID { get; set; }
    [Required]
    [ForeignKey("EquipoID")]
    public int EquipoID { get; set; }
    public Equipos? Equipo { get; set; }
    [Required]
    [ForeignKey("UsuarioID")]
    public int UsuarioID { get; set; }
    public User? Usuario { get; set; }
    [Required]
    [StringLength(255)]
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    [Required]
    public bool Publicado { get; set; }
    public DateTime? FechaCierre { get; set; }
    public int? RespuestasMaximas { get; set; }
  }
}