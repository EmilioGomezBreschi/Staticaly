using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Staticaly.Client.Models;

namespace Staticaly.Client.Models
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
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public bool? Activo { get; set; }
  }
}