using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Staticaly.Server.Models
{
    public class Rol
    {
        [Key]
        public int RolID{ get; set; }
        [Required]
        [MaxLength(50)]
        public string? RolName { get; set; }

    }
}