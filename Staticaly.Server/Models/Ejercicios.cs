using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Server.Models
{
  public class Ejercicios
  {
    [Key]
    public int EjerciciosID { get; set; }
    [Required]
    [ForeignKey("UsuarioID")]
    public int UsuarioID { get; set; }
    public User? User { get; set; }
    [Required]
    [ForeignKey("EquipoID")]
    public int EquipoID { get; set; }
    [Required]
    [StringLength(255)]
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    [Required]
    public bool Publicado { get; set; }
    public DateTime? FechaCierre { get; set; }
    public int Timer { get; set; }
    public int PuntosAquitar { get; set; }
    public bool permitirMasDeUnaRespuesta { get; set; }
    public int PuntosMin { get; set; }
  }
}