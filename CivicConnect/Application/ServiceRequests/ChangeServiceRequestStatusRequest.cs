using System.ComponentModel.DataAnnotations;

namespace Application.ServiceRequests;

public class ChangeServiceRequestStatusRequest : IValidatableObject
{
    public Guid NewStatusId { get; set; }
    public Guid ChangedByUserId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NewStatusId == Guid.Empty)
            yield return new ValidationResult("A non-empty status ID is required.", new[] { nameof(NewStatusId) });
        if (ChangedByUserId == Guid.Empty)
            yield return new ValidationResult("A non-empty actor ID is required.", new[] { nameof(ChangedByUserId) });
    }
}
