using EdwApiDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdwApiDemo.Infrastructure.EntityConfigurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Path).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.AccessRequests)
               .WithOne(x => x.Document)
               .HasForeignKey(x => x.DocumentId)
               .OnDelete(DeleteBehavior.Restrict);

        // Seed initial roles
        builder.HasData(
            new Document { Id = 1, Name = "First test document", Path = "Some path", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
            new Document { Id = 2, Name = "Second test document", Path = "Some path", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
