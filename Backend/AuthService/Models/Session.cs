using System.ComponentModel.DataAnnotations;
using AuthService.Models;

namespace AuthService;

public class Session
{
    [Key]
    public string SessionKey { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}