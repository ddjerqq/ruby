using System.ComponentModel;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ruby.Generated;

namespace Application.Items.Commands;

public sealed class CreateItemTypeCommand : IRequest<ItemType>
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public float QualityMin { get; set; }

    public float QualityMax { get; set; }

    public bool StatTrackAvailable { get; set; }

    public ItemRarity Rarity { get; set; }

    public ItemImage Image { get; set; } = default!;
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CreateItemTypeValidator : AbstractValidator<ItemType>
{
    public CreateItemTypeValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.QualityMin).InclusiveBetween(0, 1);
        RuleFor(x => x.QualityMax).InclusiveBetween(0, 1);
        RuleFor(x => x.QualityMax).GreaterThanOrEqualTo(x => x.QualityMin);
        RuleFor(x => x.Image).NotNull();
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CreateItemTypeCommandHandler(IAppDbContext dbContext, IDateTimeProvider dateTimeProvider, ICurrentUserAccessor currentUserAccessor)
    : IRequestHandler<CreateItemTypeCommand, ItemType>
{
    public async Task<ItemType> Handle(CreateItemTypeCommand request, CancellationToken ct)
    {
        var itemType = new ItemType(ItemTypeId.NewItemTypeId())
        {
            Name = request.Name,
            Description = request.Description,
            QualityMin = request.QualityMin,
            QualityMax = request.QualityMax,
            StatTrackAvailable = request.StatTrackAvailable,
            Rarity = request.Rarity,
            Image = request.Image,
            Created = dateTimeProvider.UtcNow,
            CreatedBy = currentUserAccessor.CurrentUserId.ToString(),
        };

        dbContext.Set<ItemType>().Add(itemType);
        await dbContext.SaveChangesAsync(ct);

        return itemType;
    }
}