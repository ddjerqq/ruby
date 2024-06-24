using System.Diagnostics;
using Application.Dtos;
using Application.Items.Commands;
using Application.Items.Queries;
using Application.Users.Commands;
using Application.Users.Queries;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Common;
using Ruby.Generated;

namespace Presentation.Controllers.v1;

/// <summary>
/// controller for authentication
/// </summary>
public sealed class HomeController : ApiController
{
    private IMediator Mediator => GetRequiredService<IMediator>();

    private IMapper Mapper => GetRequiredService<IMapper>();

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> Users([FromQuery] int page, [FromQuery] int perPage = 25, CancellationToken ct = default)
    {
        var users = await Mediator.Send(new GetAllUsersQuery(page, perPage), ct);
        return Ok(users.Select(Mapper.Map<UserDto>));
    }

    /// <summary>
    /// Create a random item type
    /// </summary>
    [HttpPost("create_item_type")]
    public async Task<ActionResult<ItemType>> CreateItemType([FromQuery] string name, [FromQuery] string description, CancellationToken ct)
    {
        return await Mediator.Send(new CreateRandomItemTypeCommand(name, description), ct);
    }

    /// <summary>
    /// Get all item types
    /// </summary>
    [HttpGet("get_all_item_types")]
    public async Task<ActionResult<IEnumerable<ItemType>>> ItemTypes([FromQuery] int page, [FromQuery] int perPage = 25, CancellationToken ct = default)
    {
        var itemTypes = await Mediator.Send(new GetAllItemTypesQuery(page, perPage), ct);
        return Ok(itemTypes);
    }

    /// <summary>
    /// Buy an item
    /// </summary>
    [HttpPost("buy_item")]
    public async Task<ActionResult<IEnumerable<ItemDto>>> BuyItem([FromQuery] string userId, [FromQuery] string itemTypeId, [FromQuery] int quantity, CancellationToken ct)
    {
        var itemsBought = await Mediator.Send(new BuyItemsCommand(UserId.Parse(userId), ItemTypeId.Parse(itemTypeId), quantity), ct);
        return Ok(itemsBought.Select(Mapper.Map<ItemDto>));
    }

    /// <summary>
    /// Create a new user from query
    /// </summary>
    [HttpPost("create_user")]
    public async Task<ActionResult<UserDto>> CreateUser([FromQuery] string username, CancellationToken ct)
    {
        var user = await Mediator.Send(new CreateRandomUserCommand(username), ct);
        return Ok(Mapper.Map<UserDto>(user));
    }
}