using System.ComponentModel;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ruby.Generated;

namespace Application.Items.Commands;

public sealed record BuyCaseCommand(CaseTypeId CaseTypeId) : IRequest<Case>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class BuyCaseValidator : AbstractValidator<BuyCaseCommand>
{
    public BuyCaseValidator(IAppDbContext dbContext, ICurrentUserAccessor currentUserAccessor)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleSet("async", () =>
        {
            RuleFor(x => x.CaseTypeId)
                .MustAsync(async (_, id, ctx, ct) =>
                {
                    var caseType = await dbContext.Set<CaseType>()
                        .SingleOrDefaultAsync(caseType => caseType.Id == id, ct);

                    if (caseType is null)
                    {
                        ctx.AddFailure(nameof(id), $"Case with id {id} does not exist");
                        return false;
                    }

                    var user = await currentUserAccessor.GetCurrentUserAsync(ct);
                    if (!user.Wallet.HasBalance(caseType.Price))
                    {
                        ctx.AddFailure("User does not have enough balance");
                        return false;
                    }

                    return true;
                });
        });
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class BuyCaseCommandHandler(IAppDbContext dbContext, IDateTimeProvider dateTimeProvider, ICurrentUserAccessor currentUserAccessor) : IRequestHandler<BuyCaseCommand, Case>
{
    public async Task<Case> Handle(BuyCaseCommand request, CancellationToken ct)
    {
        var caseType = await dbContext.Set<CaseType>()
            .SingleOrDefaultAsync(x => x.Id == request.CaseTypeId, ct);
        NotFoundException.ThrowIfNull(caseType, $"Case type with id {request.CaseTypeId} not found");

        var user = await currentUserAccessor.GetCurrentUserAsync(ct);

        var @case = new Case(CaseId.NewCaseId())
        {
            CaseType = caseType,
            Created = dateTimeProvider.UtcNow,
            CreatedBy = user.ToString(),
        };

        user.Wallet.TryRemoveFunds(caseType.Price);
        user.CaseInventory.Add(@case);

        await dbContext.SaveChangesAsync(ct);

        return @case;
    }
}