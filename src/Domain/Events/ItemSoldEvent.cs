using Domain.Abstractions;
using Ruby.Generated;

namespace Domain.Events;

public sealed record ItemSoldEvent(ItemId Id) : IDomainEvent;