using System.ComponentModel;
using Application.Services;
using Domain.Entities;
using MediatR;

namespace Application.Items.Commands;

public sealed record CreateRandomItemTypeCommand(string Name, string Description) : IRequest<ItemType>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CreateRandomItemTypeCommandHandler(IAppDbContext dbContext) : IRequestHandler<CreateRandomItemTypeCommand, ItemType>
{
    public async Task<ItemType> Handle(CreateRandomItemTypeCommand request, CancellationToken ct)
    {
        var itemType = ItemType.RandomItemType(request.Name, request.Description);

        dbContext.Set<ItemType>().Add(itemType);
        await dbContext.SaveChangesAsync(ct);

        return itemType;
    }
}