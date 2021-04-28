using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.Users.Queries {
    public class GetRole {
        public class Response {
            public UserRole Role { get; set; }
        }

        public class Query : IRequest<Response> {
            public int RoleId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Response> {
            private readonly IDbContextFactory<ElecriticContext> _factory;
            private readonly ILogger<QueryHandler> _logger;

            public QueryHandler(IDbContextFactory<ElecriticContext> factory, ILogger<QueryHandler> logger) {
                _factory = factory;
                _logger = logger;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken) {
                _logger.LogInformation($"Handling query {request}: {{@r}}", request);

                using var dbContext = _factory.CreateDbContext();

                return new Response {
                    Role = await dbContext.UserRoles
                        .FindAsync(request.RoleId)
                };
            }
        }
    }
}
