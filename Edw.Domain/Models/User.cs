namespace EdwApiDemo.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<AccessRequest> Requests { get; set; } = new List<AccessRequest>();
    public ICollection<Decision> Decisions { get; set; } = new List<Decision>();
}
