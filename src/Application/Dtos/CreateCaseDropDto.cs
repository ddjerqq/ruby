using Ruby.Generated;

namespace Application.Dtos;

public sealed record CreateCaseDropDto(ItemTypeId ItemTypeId, decimal DropChance, decimal DropPrice);