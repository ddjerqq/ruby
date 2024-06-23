using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.ValueConverters;

internal sealed class UlidToStringConverter() : ValueConverter<Ulid, string>(
    v => v.ToString(),
    v => Ulid.Parse(v));