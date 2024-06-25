using System.ComponentModel;
using Application.Services;
using MediatR;

namespace Application.Economy.Commands;

public sealed record WorkCommand : IRequest<int>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class WorkCommandHandler(IAppDbContext dbContext, ICurrentUserAccessor currentUserAccessor) : IRequestHandler<WorkCommand, int>
{
    public async Task<int> Handle(WorkCommand request, CancellationToken ct)
    {
        var user = await currentUserAccessor.GetCurrentUserAsync(ct);
        var amount = Random.Shared.Next(1, 1000);
        user.Wallet.AddFunds(amount);

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(ct);
        return amount;
    }
}