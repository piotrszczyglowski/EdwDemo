using EdwApiDemo.Domain.Models;

namespace EdwApiDemo.Api.Dto.DocumentAccess;

public class DecisionDto
{
    public int Id { get; set; }
    public int DecidedById { get; set; }
    public string DecidedByName { get; set; }
    public bool IsApproved { get; set; }
    public string Justification { get; set; }
    public DateTime CreatedAt { get; set; }

    public static DecisionDto FromModel(Decision model)
    {
        return new DecisionDto
        {
            Id = model.Id,
            DecidedById = model.DecidedById,
            DecidedByName = model.DecidedBy?.Name,
            IsApproved = model.IsApproved,
            Justification = model.Justification,
            CreatedAt = model.CreatedAt
        };
    }
}
