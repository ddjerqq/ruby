using System.ComponentModel;
using AutoMapper;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Application.Dtos;

public sealed record CaseDropDto
{
    public ItemTypeId ItemTypeId { get; init; }

    public string ItemName { get; init; } = default!;

    public decimal DropChance { get; init; }

    public decimal DropPrice { get; init; }
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CaseDropDtoProfile : Profile
{
    public CaseDropDtoProfile()
    {
        CreateMap<CaseDrop, CaseDropDto>()
            .ForMember(x => x.ItemTypeId, opt => opt.MapFrom(x => x.ItemType.Id))
            .ForMember(x => x.ItemName, opt => opt.MapFrom(x => x.ItemType.Name))
            .ForMember(x => x.DropChance, opt => opt.MapFrom(x => x.DropChance))
            .ForMember(x => x.DropPrice, opt => opt.MapFrom(x => x.DropPrice))
            ;
    }
}