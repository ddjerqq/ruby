using System.ComponentModel;
using AutoMapper;
using Domain.Aggregates;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Application.Dtos;

public sealed record UserDto
{
    public UserId Id { get; init; }

    public string Username { get; init; } = default!;

    public int Experience { get; init; }

    public string Level { get; init; } = default!;

    public decimal WalletBalance { get; init; }

    public int ItemCount { get; init; }

    public IEnumerable<ItemDto> Inventory { get; init; } = [];
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class UserDtoMappingProfile : Profile
{
    public UserDtoMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(u => u.Id, opt => opt.MapFrom(u => u.Id))
            .ForMember(u => u.Username, opt => opt.MapFrom(u => u.Username))
            .ForMember(u => u.Experience, opt => opt.MapFrom(u => u.Level.Value))
            .ForMember(u => u.Level, opt => opt.MapFrom(u => u.Level.DisplayName))
            .ForMember(u => u.WalletBalance, opt => opt.MapFrom(u => u.Wallet.Balance))
            .ForMember(u => u.ItemCount, opt => opt.MapFrom(u => u.Inventory.Count))
            .ForMember(u => u.Inventory, opt => opt.MapFrom(u => u.Inventory));
    }
}