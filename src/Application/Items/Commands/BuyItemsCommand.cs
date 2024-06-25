using System.ComponentModel;
using System.Diagnostics;
using Application.Services;
using Domain.Aggregates;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ruby.Generated;

namespace Application.Items.Commands;

public sealed record BuyItemsCommand(UserId UserId, ItemTypeId ItemTypeId, int Quantity) : IRequest<IEnumerable<Item>>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class BuyItemsCommandHandler(IAppDbContext dbContext, IDateTimeProvider dateTimeProvider) : IRequestHandler<BuyItemsCommand, IEnumerable<Item>>
{
    public async Task<IEnumerable<Item>> Handle(BuyItemsCommand request, CancellationToken ct)
    {
        var user = await dbContext.Set<User>()
            .Include(u => u.ItemInventory)
            .SingleOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user is null)
            throw new InvalidOperationException($"User with id: {request.UserId}");

        var itemType = await dbContext.Set<ItemType>()
            .FindAsync(request.ItemTypeId, ct);

        if (itemType is null)
            throw new InvalidOperationException($"ItemType with id: {request.ItemTypeId}");

        // TODO change this to case opening price.
        if (!user.Wallet.HasBalance(request.Quantity * 100))
            throw new InvalidOperationException($"User with id {user.Id} does not have enough balance");

        var itemsBought = Enumerable.Range(0, request.Quantity)
            .Select(_ => itemType.NewItem(user, dateTimeProvider.UtcNow, user.Id.ToString()))
            .ToList();

        Debug.Assert(user.Wallet.TryRemoveFunds(request.Quantity * 100));
        foreach (var item in itemsBought)
        {
            user.ItemInventory.Add(item);
        }

        await dbContext.SaveChangesAsync(ct);

        return itemsBought;
    }
}