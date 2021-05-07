using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.Users.Queries {
    public class Details {
        public record UserDto {
            public string Username { get; init; }
            public string Email { get; init; }
            public string FullName { get; init; }
            public string RoleName { get; init; }
        }

        public record Query : IRequest<UserDto> {
            public int UserId { get; init; }
        }

        public class Handler : IRequestHandler<Query, UserDto> {
            private readonly IDbContextFactory<ElecriticContext> _factory;
            private readonly ILogger<Handler> _logger;

            public Handler(IDbContextFactory<ElecriticContext> factory, ILogger<Handler> logger) {
                _factory = factory;
                _logger = logger;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken) {
                _logger.LogInformation($"Handling query {nameof(Details)}: {{@r}}", request);

                using var dbContext = _factory.CreateDbContext();

                return await dbContext.Users
                    .Where(u => u.Id == request.UserId)
                    .Select(u => new UserDto {
                        Username = u.Username,
                        Email = u.Email,
                        FullName = $"{u.FirstName} {u.LastName}",
                        RoleName = u.Role.Name
                    })
                    .SingleOrDefaultAsync();
            }
        }
    }
}