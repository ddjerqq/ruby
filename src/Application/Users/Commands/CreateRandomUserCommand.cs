using System.ComponentModel;
using Application.Services;
using Domain.Aggregates;
using Domain.ValueObjects;
using MediatR;
using Ruby.Generated;

namespace Application.Users.Commands;

public sealed record CreateRandomUserCommand(string Username) : IRequest<User>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CreateRandomUserCommandHandler(IAppDbContext dbContext, IDateTimeProvider dateTimeProvider) : IRequestHandler<CreateRandomUserCommand, User>
{
    public async Task<User> Handle(CreateRandomUserCommand request, CancellationToken ct)
    {
        var user = new User(UserId.NewUserId())
        {
            Username = request.Username,
            Wallet = new Wallet(Random.Shared.Next(0, 10_000)),
            Level = new Level(Random.Shared.Next(0, 1000)),
            ItemInventory = [],
            Created = dateTimeProvider.UtcNow,
            CreatedBy = "system",
        };

        dbContext.Set<User>().Add(user);
        await dbContext.SaveChangesAsync(ct);

        return user;
    }
}