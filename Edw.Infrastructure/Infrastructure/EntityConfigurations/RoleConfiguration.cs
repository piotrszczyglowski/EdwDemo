using EdwApiDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdwApiDemo.Infrastructure.EntityConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.CreatedAt).IsRequired();

        // Seed initial roles
        builder.HasData(
            new Role { Id = 1, Name = "Admin", Description = "System administrator", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
            new Role { Id = 2, Name = "Approver", Description = "Can approve document access requests", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
            new Role { Id = 3, Name = "User", Description = "Regular user", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
