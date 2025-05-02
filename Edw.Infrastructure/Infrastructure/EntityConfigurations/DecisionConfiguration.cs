using EdwApiDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdwApiDemo.Infrastructure.EntityConfigurations;

public class DecisionConfiguration : IEntityTypeConfiguration<Decision>
{
    public void Configure(EntityTypeBuilder<Decision> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Justification)
            .HasMaxLength(500);
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Configure relationship with DecidedBy user
        builder.HasOne(d => d.DecidedBy)
            .WithMany(u => u.Decisions)
            .HasForeignKey(d => d.DecidedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
