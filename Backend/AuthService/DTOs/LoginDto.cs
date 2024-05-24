using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email adress")]
    public string Email { get; set;}
    [Required]
    public string Password { get; set;}
}
