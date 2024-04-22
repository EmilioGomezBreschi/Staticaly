using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staticaly.Server.Models
{
    public class RespuestasCuestionario
    {
        [Key]
        public int RespuestasCuestionarioID { get; set; }
        [ForeignKey("CuestionarioID")]
        [Required]
        public int CuestionarioID { get; set; }
        [ForeignKey("PreguntaID")]
        [Required]
        public int PreguntaID { get; set; }
        [ForeignKey("RespuestaID")]
        [Required]
        public int RespuestaID { get; set; }
        public OpcionesCuestionario? OpcionesCuestionario { get; set; }
        [ForeignKey("UsuarioID")]
        [Required]
        public int UsuarioID { get; set; }
        public User? User { get; set; }
    }
}