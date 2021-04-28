using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.ProductDetails.Queries {
    public class GetFavorite {
        public class Response {
            public Favorite Favorite { get; set; }
        }

        public class Query : IRequest<Response> {
            public int UserId { get; set; }
            public int ProductId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Response> {
            private readonly IDbContextFactory<ElecriticContext> _factory;
            private readonly ILogger<QueryHandler> _logger;

            public QueryHandler(IDbContextFactory<ElecriticContext> factory, ILogger<QueryHandler> logger) {
                _factory = factory;
                _logger = logger;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken) {
                _logger.LogInformation($"Handling query {request}: {{@request}}", request);

                using var dbContext = _factory.CreateDbContext();

                return new Response {
                    Favorite = await dbContext.Favorites
                        .SingleOrDefaultAsync(f =>
                            f.UserId == request.UserId && f.ProductId == request.ProductId)
                };
            }
        }
    }
}
