using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Server.Models
{
    public class Comentarios
    {
        [Key]
        public int ComentarioID { get; set; }
        [Required]
        [ForeignKey("PublicacionID")]
        public int PublicacionID { get; set; }
        public Publicaciones? Publicacion { get; set; }
        [Required]
        [ForeignKey("UsuarioID")]
        public int UsuarioID { get; set; }
        public User? Usuario { get; set; }
        [Required]
        public string? Contenido { get; set; }
        public byte[]? Imagen { get; set; }
        [Required]
        public DateTime? Fecha { get; set; }
        [Required]
        public int Reportes { get; set; }
    }
}