namespace EdwApiDemo.Api.Dto.DocumentAccess;

public class ProcessAccessRequestDto
{
    public int RequestId { get; set; }
    public int ProcessedById { get; set; }
    public bool IsApproved { get; set; }
    public string Reason { get; set; }
}
