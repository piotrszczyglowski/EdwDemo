using MediatR;
using EdwApiDemo.Domain.Models;
using EdwApiDemo.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Commands.Users.Handlers;

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Unit>
{
    private readonly EdwDbContext _context;

    public AssignRoleCommandHandler(EdwDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == request.Role, cancellationToken);

        if (role == null)
        {
            throw new InvalidOperationException("Role not found");
        }

        if (!user.UserRoles.Any(ur => ur.RoleId == role.Id))
        {
            user.UserRoles.Add(new UserRole 
            { 
                UserId = user.Id,
                RoleId = role.Id,
                AssignedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}


