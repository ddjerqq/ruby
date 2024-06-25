using System.Diagnostics;
using Application.Services;
using Domain.Aggregates;
using Ruby.Generated;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Infrastructure.Services;

public sealed class HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IAppDbContext dbContext)
    : ICurrentUserAccessor
{
    public UserId? CurrentUserId
    {
        get
        {
            var stringId = httpContextAccessor
                .HttpContext?
                .User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value;

            return UserId.TryParse(stringId, out var id) ? id : null;
        }
    }

    public async Task<User?> TryGetCurrentUserAsync(CancellationToken ct = default)
    {
        var id = CurrentUserId;
        if (id is null) return null;

        if (httpContextAccessor.HttpContext!.Items.TryGetValue(nameof(User), out var cachedUser) && cachedUser is User user)
            return user;

        // this may cause a cartesian explosion, so we will use a [split query](https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries)
        var ts = Stopwatch.StartNew();
        var userFromDb = await dbContext.Users
            .Include(x => x.ItemInventory)
            .Include(x => x.CaseInventory)
            // .AsSplitQuery()
            .SingleOrDefaultAsync(u => u.Id == id, ct);
        ts.Stop();
        Debug.WriteLine($"User loaded from the database in {ts.ElapsedMilliseconds}ms");

        if (userFromDb is null)
            throw new InvalidOperationException($"Failed to load the user from the database, user with id: {id} not found");

        httpContextAccessor.HttpContext!.Items.Add(nameof(User), userFromDb);
        return userFromDb;
    }
}