using Application.Dtos;
using Application.Items.Commands;
using Application.Items.Queries;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using Ruby.Generated;

namespace Presentation.Controllers.v1;

/// <summary>
/// Controller for inventory
/// </summary>
[Authorize]
[Route("/api/v1/[controller]")]
public sealed class InventoryController(IMapper mapper, IMediator mediator) : ApiController
{
    /// <summary>
    /// Create a random item type
    /// </summary>
    [HttpPost("create_item_type")]
    public async Task<ActionResult<ItemType>> CreateItemType([FromBody] CreateItemTypeCommand command, CancellationToken ct)
    {
        var itemType = await mediator.Send(command, ct);
        return Ok(itemType);
    }

    /// <summary>
    /// Get all item types
    /// </summary>
    [HttpGet("get_all_item_types")]
    public async Task<ActionResult<IEnumerable<ItemType>>> ItemTypes([FromQuery] int page = 0, [FromQuery] int perPage = 25, CancellationToken ct = default)
    {
        var itemTypes = await mediator.Send(new GetAllItemTypesQuery(page, perPage), ct);
        return Ok(itemTypes);
    }

    /// <summary>
    /// Create a case type
    /// </summary>
    [HttpPost("create_case_type")]
    public async Task<ActionResult<CaseDto>> CreateCaseType([FromBody] CreateCaseTypeCommand command, CancellationToken ct)
    {
        var caseDto = await mediator.Send(command, ct);
        return Ok(caseDto);
    }

    /// <summary>
    /// Get all case types
    /// </summary>
    [HttpGet("get_all_case_types")]
    public async Task<ActionResult<IEnumerable<CaseType>>> CaseTypes([FromQuery] int page = 0, [FromQuery] int perPage = 25, CancellationToken ct = default)
    {
        var caseTypes = await mediator.Send(new GetAllCaseTypesQuery(page, perPage), ct);
        return Ok(caseTypes.Select(mapper.Map<CaseTypeDto>));
    }

    /// <summary>
    /// Buy a case
    /// </summary>
    [HttpPost("buy_case/{caseTypeId}")]
    public async Task<ActionResult<CaseDto>> BuyCase(string caseTypeId, CancellationToken ct)
    {
        var @case = await mediator.Send(new BuyCaseCommand(CaseTypeId.Parse(caseTypeId)), ct);
        return Ok(mapper.Map<CaseDto>(@case));
    }

    /// <summary>
    /// Open a case
    /// </summary>
    [HttpPost("open/{caseId}")]
    public async Task<ActionResult<ItemDto>> OpenCase(string caseId, CancellationToken ct)
    {
        var item = await mediator.Send(new OpenCaseCommand(CaseId.Parse(caseId)), ct);
        return Ok(mapper.Map<ItemDto>(item));
    }
}