using System.ComponentModel;
using AutoMapper;
using Domain.Entities;
using Ruby.Generated;

namespace Application.Dtos;

public sealed record CaseTypeDto
{
    public CaseTypeId Id { get; init; }

    public string Name { get; init; } = default!;

    public string ImageUrl { get; init; } = default!;

    public ICollection<CaseDropDto> Drops { get; init; } = [];

    public decimal Price { get; init; }
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CaseTypeDtoProfile : Profile
{
    public CaseTypeDtoProfile()
    {
        CreateMap<CaseType, CaseTypeDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.ImageUrl))
            .ForMember(x => x.Drops, opt => opt.MapFrom(x => x.Drops))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Price));
    }
}