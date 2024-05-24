namespace AuthService.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public ICollection<Session> Sessions{ get; set; }
}
