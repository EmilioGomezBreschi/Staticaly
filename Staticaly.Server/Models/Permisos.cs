using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Staticaly.Server.Models
{
    public class Permisos
    {
        [Key]
        public int PermisoID { get; set; }
        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }
    }
}