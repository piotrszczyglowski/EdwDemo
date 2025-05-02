using EdwApiDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdwApiDemo.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.UserRoles)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Requests)
               .WithOne(x => x.RequestedBy)
               .HasForeignKey(x => x.RequestedById)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Decisions)
               .WithOne(x => x.DecidedBy)
               .HasForeignKey(x => x.DecidedById)
               .OnDelete(DeleteBehavior.Restrict);

        // Unique index na Email
        builder.HasIndex(x => x.Email)
               .IsUnique();


        // Seed initial roles
        builder.HasData(
            new User { Id = 1, Name = "Admin", Email = "admin@edw.com", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 2, Name = "Approver", Email = "Approver@edw.com", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 3, Name = "User", Email = "User@edw.com", CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
