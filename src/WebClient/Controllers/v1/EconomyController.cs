using Application.Economy.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers.v1;

/// <summary>
/// controller for economy
/// </summary>
[Authorize]
[Route("/api/v1/economy")]
public sealed class EconomyController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Work and earn a random amount
    /// </summary>
    [HttpPost("work")]
    public async Task<ActionResult<decimal>> Work(CancellationToken ct)
    {
        var amount = await mediator.Send(new WorkCommand(), ct);
        return Ok(amount);
    }
}