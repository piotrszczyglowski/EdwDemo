using EdwApiDemo.Domain.Models;
using System.Numerics;

namespace EdwApiDemo.Api.Dto.DocumentAccess;

public class AccessRequestDto
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public string DocumentName { get; set; }
    public int RequestedById { get; set; }
    public string RequestedByName { get; set; }
    public string Status { get; set; }
    public string Comments { get; set; }
    public string AccessType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DecisionDto Decision { get; set; }

    public static AccessRequestDto FromModel(AccessRequest model)
    {
        return new AccessRequestDto
        {
            Id = model.Id,
            DocumentId = model.DocumentId,
            DocumentName = model.Document?.Name,
            RequestedById = model.RequestedById,
            RequestedByName = model.RequestedBy?.Name,
            Status = model.Status.ToString(),
            Comments = model.Comments,
            AccessType = model.AccessType.ToString(),
            CreatedAt = model.CreatedAt,
            Decision = model.Decision != null ? DecisionDto.FromModel(model.Decision) : null
        };
    }
}
