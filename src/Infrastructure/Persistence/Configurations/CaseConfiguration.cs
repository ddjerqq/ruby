using System.ComponentModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.HasOne(@case => @case.CaseType)
            .WithMany()
            .HasForeignKey(@case => @case.CaseTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // a case does not exist without a case type
        builder.Navigation(@case => @case.CaseType)
            .AutoInclude();

        builder.HasOne(@case => @case.Owner)
            .WithMany(user => user.CaseInventory)
            .HasForeignKey(@case => @case.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}