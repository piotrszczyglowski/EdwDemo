using EdwApiDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Infrastructure.DbContext;

public class EdwDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Document> Documents { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<AccessRequest> AccessRequests { get; set; }
    public DbSet<Decision> Decisions { get; set; }

    public EdwDbContext(DbContextOptions<EdwDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EdwDbContext).Assembly);
    }
}
