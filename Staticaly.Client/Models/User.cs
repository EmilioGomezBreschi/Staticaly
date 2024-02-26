using System.ComponentModel.DataAnnotations;

namespace Staticaly.Client.Models
{
  public class User
  {
    [Key]
    public int UsuarioID { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Nombre { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Apellido { get; set; }
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y tener una longitud mínima de 6 caracteres.")]
    public string? Password { get; set; }
    [Required]
    public int RolID { get; set; }
    public Rol? Rol { get; set; }
    [Required]
    public int RangoID { get; set;}
    public Rango? Rango { get; set; }
    [Required]
    public int Puntos { get; set; }
    public byte[]? Imagen { get; set; }
    [MaxLength(100)]
    public string? VerificationToken { get; set; }
    public bool EmailVerified { get; set; }
  }
}
