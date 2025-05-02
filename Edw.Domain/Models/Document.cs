namespace EdwApiDemo.Domain.Models;

public class Document
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<AccessRequest> AccessRequests { get; set; } = new List<AccessRequest>();
}
