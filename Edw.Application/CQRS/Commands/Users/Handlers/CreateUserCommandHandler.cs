using EdwApiDemo.Domain.Models;
using EdwApiDemo.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Commands.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly EdwDbContext _context;
    public CreateUserCommandHandler(EdwDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new User
        {
            Email = request.Email,
            Name = request.Name,
          
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}


