using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Features.Login.Models;
using Elecritic.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.Login.Queries {
    public class GetUser {
        public class UserDto {
            public int Id { get; set; }
            public string Name { get; set; }
            public UserRole Role { get; set; }
        }

        public class Response {
            public UserDto UserDto { get; set; }
        }

        public class Query : LoginForm, IRequest<Response> { }

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
                    UserDto = await dbContext.Users
                        .Where(u => u.Email == request.Email && u.Password == request.Password)
                        .Select(u => new UserDto {
                            Id = u.Id,
                            Name = u.Username,
                            Role = u.Role
                        })
                        .SingleOrDefaultAsync()
                };
            }
        }
    }
}
