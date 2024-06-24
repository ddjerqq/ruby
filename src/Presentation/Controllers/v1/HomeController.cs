using System.Diagnostics;
using Application.Dtos;
using Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Aggregates;
using Domain.Entities;
using Domain.ValueObjects;
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
    private IAppDbContext DbContext => GetRequiredService<IAppDbContext>();

    private IMapper Mapper => GetRequiredService<IMapper>();

    private IDateTimeProvider DateTimeProvider => GetRequiredService<IDateTimeProvider>();

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> Users(CancellationToken ct)
    {
        var users = await DbContext.Set<User>()
            .ProjectTo<UserDto>(Mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return Ok(users);
    }

    /// <summary>
    /// Create a random item type
    /// </summary>
    [HttpPost("create_item_type")]
    public async Task<ActionResult<ItemType>> CreateItemType(CancellationToken ct)
    {
        var itemType = ItemType.RandomItemType();

        DbContext.Set<ItemType>().Add(itemType);
        await DbContext.SaveChangesAsync(ct);

        return Ok(itemType);
    }

    /// <summary>
    /// Get all item types
    /// </summary>
    [HttpGet("get_all_item_types")]
    public async Task<ActionResult<IEnumerable<ItemType>>> ItemTypes(CancellationToken ct)
    {
        var itemTypes = await DbContext.Set<ItemType>()
            .ToListAsync(ct);

        return Ok(itemTypes);
    }

    // buy item
    /// <summary>
    /// Buy an item
    /// </summary>
    [HttpPost("buy_item")]
    public async Task<ActionResult<ItemDto>> BuyItem([FromQuery] string userId, [FromQuery] string itemTypeId, CancellationToken ct)
    {
        var user = await DbContext.Set<User>()
            .Include(u => u.Inventory)
            .FirstOrDefaultAsync(u => u.Id == UserId.Parse(userId), ct);

        if (user is null)
            return NotFound();

        var itemType = await DbContext.Set<ItemType>()
            .FirstOrDefaultAsync(x => x.Id == ItemTypeId.Parse(itemTypeId), ct);

        if (itemType is null)
            return NotFound();

        if (user.Wallet.Balance < 100)
            return BadRequest("Not enough balance");

        var item = itemType.NewItem(user, DateTimeProvider.UtcNow, user.Id.ToString());

        Debug.Assert(user.Wallet.TryRemoveFunds(100));
        user.Inventory.Add(item);

        await DbContext.SaveChangesAsync(ct);

        return Ok(Mapper.Map<ItemDto>(item));
    }

    /// <summary>
    /// Create a new user from query
    /// </summary>
    [HttpPost("create_user")]
    public async Task<ActionResult<UserDto>> CreateUser([FromQuery] string username, CancellationToken ct)
    {
        var user = new User(UserId.NewUserId())
        {
            Username = username,
            Wallet = new Wallet(Random.Shared.Next(0, 10_000)),
            Level = new Level(Random.Shared.Next(0, 1000)),
            Inventory = [],
            Created = DateTimeProvider.UtcNow,
            CreatedBy = "system",
        };

        DbContext.Set<User>().Add(user);
        await DbContext.SaveChangesAsync(ct);

        return Ok(Mapper.Map<UserDto>(user));
    }
}