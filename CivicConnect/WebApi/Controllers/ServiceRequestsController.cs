using Application.Abstractions;
using Application.ServiceRequests;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestLifecycleService
            _lifecycleService;

        public ServiceRequestsController(
            IServiceRequestLifecycleService lifecycleService)
        {
            _lifecycleService = lifecycleService;
        }

        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus(
    Guid id,
    [FromBody] ChangeServiceRequestStatusRequest request,
    CancellationToken cancellationToken)
        {
            try
            {
                await _lifecycleService.ChangeStatusAsync(
                    id,
                    request.NewStatusId,
                    request.ChangedByUserId,
                    cancellationToken);

                return NoContent();
            }
            catch (InvalidStatusTransitionException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
