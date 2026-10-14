namespace WebApi.Contracts;

public sealed record StatusDto(Guid StatusId, string StatusName, string Description, bool IsActive);
