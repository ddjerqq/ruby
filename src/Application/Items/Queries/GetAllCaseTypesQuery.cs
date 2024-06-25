using System.ComponentModel;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Queries;

public sealed record GetAllCaseTypesQuery(int Page, int PerPage) : IRequest<IEnumerable<CaseType>>;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class GetAllCaseTypesQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetAllCaseTypesQuery, IEnumerable<CaseType>>
{
    public async Task<IEnumerable<CaseType>> Handle(GetAllCaseTypesQuery request, CancellationToken ct)
    {
        return await dbContext.Set<CaseType>()
            .OrderBy(x => x.Id)
            .Skip(request.Page * request.PerPage)
            .Take(request.PerPage)
            .ToListAsync(ct);
    }
}