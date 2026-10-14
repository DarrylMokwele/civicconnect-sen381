namespace WebApi.Contracts;

public sealed class AdminAssignmentRequest
{
    public Guid DepartmentId { get; set; }
    public Guid? AssignedUserId { get; set; }
}
