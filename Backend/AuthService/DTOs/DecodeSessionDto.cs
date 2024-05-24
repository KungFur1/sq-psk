using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs;

public class DecodeSessionDto
{
    [Required]
    public string SessionKey { get; set;}
}
