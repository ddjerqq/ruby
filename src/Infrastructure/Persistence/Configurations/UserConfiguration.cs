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

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.Property(e => e.Email)
            .HasMaxLength(128);

        builder.Property(e => e.PasswordHash)
            .HasMaxLength(60);

        builder.Property(e => e.Level)
            .HasConversion(level => level.Value, exp => new Level(exp));

        builder.Property(e => e.Wallet)
            .HasConversion(wallet => wallet.Balance, balance => new Wallet(balance));

        builder.HasMany(e => e.ItemInventory)
            .WithOne(item => item.Owner)
            .HasForeignKey(item => item.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.CaseInventory)
            .WithOne(@case => @case.Owner)
            .HasForeignKey(@case => @case.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}