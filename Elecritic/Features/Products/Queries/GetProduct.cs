using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.Products.Queries {
    public class GetProduct {
        public class Response {
            public Product Product { get; set; }
        }

        public class Query : IRequest<Response> {
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
                _logger.LogInformation($"Handling {request}: {{@request}}", request);

                using var dbContext = _factory.CreateDbContext();

                return new Response {
                    Product = await dbContext.Products
                        .Select(p => new Product {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            ImagePath = p.ImagePath,
                            ReleaseDate = p.ReleaseDate,
                            Category = p.Category,
                            Company = p.Company,
                            Reviews = p.Reviews.Select(r => new Review {
                                Title = r.Title,
                                Text = r.Text,
                                Rating = r.Rating,
                                PublishDate = r.PublishDate,
                                User = new User {
                                    Username = r.User.Username
                                }
                            })
                            .ToList()
                        })
                        .SingleOrDefaultAsync(p => p.Id == request.ProductId)
                };
            }
        }
    }
}