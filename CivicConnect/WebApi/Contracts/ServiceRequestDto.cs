namespace WebApi.Contracts;

public sealed record ServiceRequestDto(
    Guid ServiceRequestId,
    Guid RequesterId,
    Guid StatusId,
    string StatusName,
    Guid? DepartmentId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime UpdatedAt);
