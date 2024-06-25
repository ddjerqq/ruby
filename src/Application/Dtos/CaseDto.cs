using System.ComponentModel;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Application.Dtos;

public sealed record CaseDto
{
    public CaseId Id { get; init; }

    public string Name { get; init; } = default!;

    public string ImageUrl { get; init; } = default!;

    public bool IsOpened { get; init; }

    public DateTime? OpenedOn { get; init; }

    public UserId OwnerId { get; init; }

    public decimal Price { get; init; }

    public IEnumerable<CaseDropDto> Drops { get; init; } = default!;
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CaseDtoProfile : Profile
{
    public CaseDtoProfile()
    {
        CreateMap<Case, CaseDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.CaseType.Name))
            .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.CaseType.ImageUrl))
            .ForMember(x => x.IsOpened, opt => opt.MapFrom(x => x.IsOpened))
            .ForMember(x => x.OpenedOn, opt => opt.MapFrom(x => x.OpenedOn))
            .ForMember(x => x.OwnerId, opt => opt.MapFrom(x => x.OwnerId))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.CaseType.Price))
            .ForMember(x => x.Drops, opt => opt.MapFrom(x => x.CaseType.Drops));
    }
}