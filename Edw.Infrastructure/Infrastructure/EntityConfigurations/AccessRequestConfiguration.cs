using EdwApiDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdwApiDemo.Infrastructure.EntityConfigurations;

public class AccessRequestConfiguration : IEntityTypeConfiguration<AccessRequest>
{
    public void Configure(EntityTypeBuilder<AccessRequest> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Comments)
            .HasMaxLength(500);
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Configure one-to-one relationship with Decision
        builder.HasOne(ar => ar.Decision)
            .WithOne(d => d.AccessRequest)
            .HasForeignKey<Decision>(d => d.AccessRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure other relationships
        builder.HasOne(ar => ar.Document)
            .WithMany(d => d.AccessRequests)
            .HasForeignKey(ar => ar.DocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ar => ar.RequestedBy)
            .WithMany(u => u.Requests)
            .HasForeignKey(ar => ar.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
