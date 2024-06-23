using System.ComponentModel;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Application.Dtos;

public sealed record ItemDto
{
    public ItemId Id { get; init; }

    public string DisplayName { get; init; } = default!;

    public float Quality { get; init; }

    public bool IsStatTrack { get; init; }

    public ItemImage Image { get; init; } = default!;

    public ItemRarity Rarity { get; init; }

    public UserId OwnerId { get; init; }
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class ItemDtoMappingProfile : Profile
{
    public ItemDtoMappingProfile()
    {
        CreateMap<Item, ItemDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.DisplayName, opt => opt.MapFrom(x => $"{x.Quality.DisplayName} {x.Name}"))
            .ForMember(x => x.Quality, opt => opt.MapFrom(x => x.Quality.Value))
            .ForMember(x => x.IsStatTrack, opt => opt.MapFrom(x => x.Quality.IsStatTrack))
            .ForMember(x => x.Image, opt => opt.MapFrom(x => x.Image))
            .ForMember(x => x.Rarity, opt => opt.MapFrom(x => x.Rarity))
            .ForMember(x => x.OwnerId, opt => opt.MapFrom(x => x.OwnerId));
    }
}