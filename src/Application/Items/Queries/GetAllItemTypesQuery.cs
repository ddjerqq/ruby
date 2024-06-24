using System.ComponentModel;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Queries;

public sealed record GetAllItemTypesQuery(int Page, int PerPage) : IRequest<IEnumerable<ItemType>>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class GetAllItemTypesQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetAllItemTypesQuery, IEnumerable<ItemType>>
{
    public async Task<IEnumerable<ItemType>> Handle(GetAllItemTypesQuery request, CancellationToken ct)
    {
        return await dbContext.Set<ItemType>()
            .OrderBy(x => x.Id)
            .Skip(request.Page * request.PerPage)
            .Take(request.PerPage)
            .ToListAsync(ct);
    }
}