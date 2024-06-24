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

    public ItemTypeId ItemTypeId { get; init; }

    public string Description { get; init; } = default!;

    public float Quality { get; init; }

    public string QualityName { get; init; } = default!;

    public bool IsStatTrack { get; init; }

    public ItemImage Image { get; init; } = default!;

    public ItemRarity Rarity { get; init; }

    public UserId OwnerId { get; init; }

    public string OwnerUsername { get; init; } = default!;

    public DateTime Created { get; init; }

    public string CreatedBy { get; init; } = default!;
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class ItemDtoMappingProfile : Profile
{
    public ItemDtoMappingProfile()
    {
        CreateMap<Item, ItemDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.DisplayName, opt => opt.MapFrom(x => $"{x.Quality.DisplayName} {x.Type.Name}"))
            .ForMember(x => x.ItemTypeId, opt => opt.MapFrom(x => x.Type.Id))
            .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Type.Description))
            .ForMember(x => x.Quality, opt => opt.MapFrom(x => x.Quality.Value))
            .ForMember(x => x.QualityName, opt => opt.MapFrom(x => x.Quality.DisplayName))
            .ForMember(x => x.IsStatTrack, opt => opt.MapFrom(x => x.Quality.IsStatTrack))
            .ForMember(x => x.Image, opt => opt.MapFrom(x => x.Type.Image))
            .ForMember(x => x.Rarity, opt => opt.MapFrom(x => x.Type.Rarity))
            .ForMember(x => x.OwnerId, opt => opt.MapFrom(x => x.Owner.Id))
            .ForMember(x => x.OwnerUsername, opt => opt.MapFrom(x => x.Owner.Username))
            .ForMember(x => x.Created, opt => opt.MapFrom(x => x.Created))
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom(x => x.CreatedBy));
    }
}