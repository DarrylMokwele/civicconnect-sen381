namespace WebApi.Contracts;

public sealed record NotificationDto(
    Guid NotificationId,
    Guid ServiceRequestId,
    string NotificationType,
    string Message,
    DateTime CreatedAt,
    string Status);
