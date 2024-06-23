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
            Inventory = Enumerable.Range(0, Random.Shared.Next(0, 10)).Select(_ => Item.RandomItem()).ToList(),
            Created = DateTimeProvider.UtcNow,
            CreatedBy = "system",
        };

        DbContext.Set<User>().Add(user);
        await DbContext.SaveChangesAsync(ct);

        return Ok(Mapper.Map<UserDto>(user));
    }
}