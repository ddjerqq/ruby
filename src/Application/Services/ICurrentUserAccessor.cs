using Application.Exceptions;
using Domain.Aggregates;
using Ruby.Generated;

namespace Application.Services;

public interface ICurrentUserAccessor
{
    public UserId? CurrentUserId { get; }

    public virtual async Task<User> GetCurrentUserAsync(CancellationToken ct = default) =>
        await TryGetCurrentUserAsync(ct) ?? throw new UnauthenticatedException("The user was not authenticated");

    public Task<User?> TryGetCurrentUserAsync(CancellationToken ct = default);
}