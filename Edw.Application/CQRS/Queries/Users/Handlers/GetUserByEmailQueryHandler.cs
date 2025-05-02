using MediatR;
using EdwApiDemo.Application.CQRS.Queries.Users.DTOs;
using EdwApiDemo.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Queries.Users.Handlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly EdwDbContext _context;

    public GetUserByEmailQueryHandler(EdwDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => EF.Functions.Collate(u.Email, "NOCASE") == request.Email, cancellationToken);

        if (user == null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name
        };
    }
}


