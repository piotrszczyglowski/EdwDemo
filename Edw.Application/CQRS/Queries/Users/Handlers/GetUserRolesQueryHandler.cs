using MediatR;
using EdwApiDemo.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Queries.Users.Handlers;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IEnumerable<string>>
{
    private readonly EdwDbContext _context;

    public GetUserRolesQueryHandler(EdwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == request.UserId)
            .Select(ur => ur.Role.Name)
            .ToListAsync(cancellationToken);
    }
}


