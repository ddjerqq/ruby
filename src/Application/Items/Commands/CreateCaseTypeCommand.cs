using System.ComponentModel;
using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ruby.Generated;

namespace Application.Items.Commands;

public sealed record CreateCaseTypeCommand(
    string Name,
    string ImageUrl,
    IEnumerable<CreateCaseDropDto> Drops) : IRequest<CaseType>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CreateCaseTypeValidator : AbstractValidator<CaseType>
{
    public CreateCaseTypeValidator(IAppDbContext dbContext)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.Drops)
            .NotEmpty()
            .Must(drops => drops.Sum(drop => drop.DropChance) == 1m)
            .WithMessage("Drop chances must sum to 1")
            .Must(drops => drops.Count() is >= 2 and <= 10)
            .WithMessage("Case must have between 2 and 10 drops");

        RuleSet("async", () =>
        {
            RuleFor(x => x.Name)
                .MustAsync(async (name, ct) =>
                {
                    var itemTypeCount = await dbContext.Set<CaseType>().CountAsync(x => x.Name == name, ct);
                    return itemTypeCount == 0;
                })
                .WithMessage("Case with this name already exists");
        });
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CreateCaseTypeCommandHandler(IAppDbContext dbContext, IDateTimeProvider dateTimeProvider, ICurrentUserAccessor currentUserAccessor) : IRequestHandler<CreateCaseTypeCommand, CaseType>
{
    public async Task<CaseType> Handle(CreateCaseTypeCommand request, CancellationToken ct)
    {
        var id = CaseTypeId.NewCaseTypeId();

        var caseType = new CaseType(id)
        {
            Name = request.Name,
            ImageUrl = request.ImageUrl,
            Created = dateTimeProvider.UtcNow,
            CreatedBy = currentUserAccessor.CurrentUserId.ToString(),
            Drops = request.Drops.Select(dto => new CaseDrop
            {
                CaseTypeId = id,
                ItemTypeId = dto.ItemTypeId,
                DropChance = dto.DropChance,
                DropPrice = dto.DropPrice,
            }).ToList(),
        };

        dbContext.Set<CaseType>().Add(caseType);
        await dbContext.SaveChangesAsync(ct);

        return caseType;
    }
}