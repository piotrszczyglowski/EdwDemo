using EdwApiDemo.Domain.Models;

namespace EdwApiDemo.Api.Dto.DocumentAccess;

/// <summary>
/// Data transfer object for creating a document access request
/// </summary>
public class CreateAccessRequestDto
{
    /// <summary>
    /// The reason for requesting access to the document
    /// </summary>
    public string Reason { get; set; }
    
    /// <summary>
    /// The type of access requested (Read or Edit)
    /// </summary>
    public AccessType AccessType { get; set; }
}
