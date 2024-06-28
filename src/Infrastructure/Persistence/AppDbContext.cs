using Application.Common;
using Application.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ruby.Common.PrimitiveExt;
using Ruby.Generated;

namespace Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options, ConvertDomainEventsToOutboxMessagesInterceptor convertDomainEventsToOutboxMessagesInterceptor) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Item> Items => Set<Item>();

    public DbSet<ItemType> ItemTypes => Set<ItemType>();

    public DbSet<Case> Cases => Set<Case>();

    public DbSet<CaseDrop> CaseDrops => Set<CaseDrop>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Infrastructure.Assembly);
        base.OnModelCreating(builder);
        SnakeCaseRename(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(convertDomainEventsToOutboxMessagesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder
            .Properties<DateTime>()
            .HaveConversion<DateTimeUtcValueConverter>();

        builder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();

        builder
            .Properties<ItemQualityType>()
            .HaveConversion<EnumToStringConverter<ItemQualityType>>();

        builder
            .Properties<ItemRarity>()
            .HaveConversion<EnumToStringConverter<ItemRarity>>();

        // source generators
        builder.ConfigureUserIdConventions();
        builder.ConfigureItemIdConventions();
        builder.ConfigureItemTypeIdConventions();
        builder.ConfigureCaseIdConventions();
        builder.ConfigureCaseTypeIdConventions();
        builder.ConfigureOutboxMessageIdConventions();

        base.ConfigureConventions(builder);
    }

    private static void SnakeCaseRename(ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var entityTableName = entity.GetTableName()!
                .Replace("AspNet", string.Empty)
                .TrimEnd('s')
                .ToSnakeCase();

            entity.SetTableName(entityTableName);

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.GetColumnName().ToSnakeCase());

            foreach (var key in entity.GetKeys())
                key.SetName(key.GetName()!.ToSnakeCase());

            foreach (var key in entity.GetForeignKeys())
                key.SetConstraintName(key.GetConstraintName()!.ToSnakeCase());

            foreach (var index in entity.GetIndexes())
                index.SetDatabaseName(index.GetDatabaseName()!.ToSnakeCase());
        }
    }
}