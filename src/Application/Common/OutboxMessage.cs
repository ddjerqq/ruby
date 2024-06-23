using System.ComponentModel.DataAnnotations;
using Application.Services;
using Domain.Abstractions;
using Newtonsoft.Json;
using Ruby.Generated;

namespace Application.Common;

[StrongId(typeof(Ulid))]
public sealed class OutboxMessage
{
    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy(),
        },
    };

    public OutboxMessageId Id { get; init; } = OutboxMessageId.NewOutboxMessageId();

    [StringLength(128)]
    public string Type { get; init; } = default!;

    [StringLength(1024)]
    public string Content { get; init; } = string.Empty;

    public DateTime OccuredOnUtc { get; init; }

    public DateTime? ProcessedOnUtc { get; set; }

    [StringLength(1024)]
    public string? Error { get; set; }

    public static OutboxMessage FromDomainEvent(IDomainEvent domainEvent, IDateTimeProvider dateTimeProvider)
    {
        return new OutboxMessage
        {
            Id = OutboxMessageId.NewOutboxMessageId(),
            Type = domainEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings),
            OccuredOnUtc = dateTimeProvider.UtcNow,
            ProcessedOnUtc = null,
            Error = null,
        };
    }
}