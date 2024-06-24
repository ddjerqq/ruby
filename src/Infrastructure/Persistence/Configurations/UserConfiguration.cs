using System.ComponentModel;
using Domain.Aggregates;
using Domain.ValueObjects;
using Ruby.Generated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.HasIndex(e => e.Username)
            .IsUnique();

        builder.Property(e => e.Username)
            .HasMaxLength(32);

        builder.Property(e => e.Level)
            .HasConversion(
                level => level.Value,
                exp => new Level(exp));

        builder.Property(e => e.Wallet)
            .HasConversion(
                wallet => wallet.Balance,
                balance => new Wallet(balance));

        builder.HasMany(e => e.Inventory)
            .WithOne(item => item.Owner);

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<User> builder)
    {
        // dt: 2024-01-01
        // ts: 1704067200
        // id: 0001JS40400000000000000000

        var user0 = new User(UserId.Parse("user_0001js40400000000000000000"))
        {
            Username = "ddjerqq",
            Level = new Level(1),
            Wallet = new Wallet(1000),
            Created = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "system",
        };

        builder.HasData(user0);
    }
}