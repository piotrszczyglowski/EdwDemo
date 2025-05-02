namespace EdwApiDemo.Domain.Models;

public class Decision
{
    public int Id { get; set; }
    public int AccessRequestId { get; set; }
    public AccessRequest AccessRequest { get; set; }
    public int DecidedById { get; set; }
    public User DecidedBy { get; set; }
    public bool IsApproved { get; set; }
    public string Justification { get; set; }
    public DateTime CreatedAt { get; set; }
}
