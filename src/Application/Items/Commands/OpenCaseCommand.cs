using System.ComponentModel;
using Application.Services;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Ruby.Generated;

namespace Application.Items.Commands;

public sealed record OpenCaseCommand(CaseId CaseId) : IRequest<Item>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class OpenCaseValidator : AbstractValidator<OpenCaseCommand>
{
    public OpenCaseValidator(ICurrentUserAccessor currentUserAccessor)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CaseId).NotEmpty();

        RuleSet("async", () =>
        {
            RuleFor(x => x.CaseId)
                .MustAsync(async (_, id, ctx, ct) =>
                {
                    var user = await currentUserAccessor.GetCurrentUserAsync(ct);
                    var @case = user.CaseInventory.SingleOrDefault(@case => @case.Id == id);

                    if (@case is null)
                    {
                        ctx.AddFailure("User does not own this case");
                        return false;
                    }

                    if (@case.IsOpened)
                    {
                        ctx.AddFailure("Case is already opened");
                        return false;
                    }

                    return true;
                });
        });
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class OpenCaseHandler(IAppDbContext dbContext, IDateTimeProvider dateTimeProvider, ICurrentUserAccessor currentUserAccessor) : IRequestHandler<OpenCaseCommand, Item>
{
    public async Task<Item> Handle(OpenCaseCommand request, CancellationToken ct)
    {
        var user = await currentUserAccessor.GetCurrentUserAsync(ct);

        var @case = user.CaseInventory.First(@case => @case.Id == request.CaseId);
        var drop = @case.Open(dateTimeProvider.UtcNow, user.Id.ToString());

        var droppedItemType = drop.ItemType;
        var item = droppedItemType.NewItem(user, dateTimeProvider.UtcNow, user.Id.ToString());

        user.ItemInventory.Add(item);
        await dbContext.SaveChangesAsync(ct);

        return item;
    }
}