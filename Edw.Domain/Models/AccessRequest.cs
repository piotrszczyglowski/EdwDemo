namespace EdwApiDemo.Domain.Models;

public class AccessRequest
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public Document Document { get; set; }
    public int RequestedById { get; set; }
    public User RequestedBy { get; set; }
    public RequestStatus Status { get; set; }
    public string Comments { get; set; }
    public AccessType AccessType { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? DecisionId { get; set; }
    public Decision Decision { get; set; }
}

public enum RequestStatus
{
    Pending,
    Approved,
    Rejected
}

public enum AccessType
{
    Read,
    Edit
}
