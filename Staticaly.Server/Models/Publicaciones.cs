using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Server.Models
{
    public class Publicaciones
    {
      [Key]
      public int PublicacionID { get; set; }
      [Required]
      [ForeignKey("EquipoID")]
      public int ForoID { get; set; }
      [Required]
      [ForeignKey("UsuarioID")]
      public int UsuarioID { get; set; }
      public User? Usuario { get; set; }
      [Required]
      [StringLength(255)]
      public string? Titulo { get; set; }
      [Required]
      public string? Contenido { get; set; }
      public byte[]? Imagen { get; set; }
      public float Calificacion { get; set; }
      [Required]
      public int Reportes { get; set; }
      [Required]
      public DateTime Fecha { get; set; }
      [Required]
      public bool? Aprobado { get; set; }
    }
}