namespace WebApi.Contracts;

public sealed class CreateServiceRequestRequest
{
    public Guid RequesterId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
