using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs;

public class SignUpDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email adress")]
    public string Email { get; set;}
    [Required]
    [Length(minimumLength: 6, maximumLength: 30, ErrorMessage = "Invalid password length")]
    public string Password { get; set;}
    [Required]
    [Length(minimumLength: 4, maximumLength: 30, ErrorMessage = "Invalid username length")]
    public string UserName { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}