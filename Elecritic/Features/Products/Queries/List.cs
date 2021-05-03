using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.Products.Queries {
    public class List {
        public class ProductDto {
            public int Id { get; set; }
            public string Name { get; set; }
            public int CategoryId { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public double AverageRating { get; set; }
        }

        public class Response {
            public List<ProductDto> Products { get; set; }
        }

        public class Query : IRequest<Response> {
            public int? TopFavorites { get; set; }
            public int? TopPopular { get; set; }
            public int? CategoryId { get; set; }
            public int? SkipNumber { get; set; }
            public int? TakeNumber { get; set; }
            public int? FavoritesByUserId { get; set; }
            public bool? All { get; set; }
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
                var products = dbContext.Products.AsQueryable();

                if (request.TopFavorites is not null) {
                    _logger.LogInformation($"Getting {request.TopFavorites} top favorite products.");

                    products = products
                        .OrderByDescending(p => p.Favorites.Count)
                        .Take((int)request.TopFavorites);
                }
                else if (request.TopPopular is not null) {
                    _logger.LogInformation($"Getting {request.TopPopular} top popular products.");

                    products = products
                        .OrderByDescending(p => p.Reviews.Count)
                        .Take((int)request.TopPopular);
                }
                else if (request.All is not null and true) {
                    _logger.LogInformation("Getting all products.");
                }
                else {
                    if (request.CategoryId is not null) {
                        _logger.LogInformation($"Getting products by category ID: {request.CategoryId}");

                        products = products
                            .Where(p => p.CategoryId == (int)request.CategoryId);
                    }
                    if (request.FavoritesByUserId is not null) {
                        _logger.LogInformation($"Getting favorite products of user ID: {request.FavoritesByUserId}");

                        products = products
                            .Where(p => p.Favorites
                                .Any(f => f.UserId == (int)request.FavoritesByUserId));
                    }
                    if (request.SkipNumber is not null && request.TakeNumber is not null) {
                        products = products
                            .OrderBy(p => p.Id)
                            .Skip((int)request.SkipNumber)
                            .Take((int)request.TakeNumber);
                    }
                }

                return new Response {
                    Products = await products
                        .Select(p => new ProductDto {
                            Id = p.Id,
                            Name = p.Name,
                            CategoryId = p.CategoryId,
                            Description = p.Description,
                            ImagePath = p.ImagePath,
                            AverageRating = p.Reviews.Count == 0 ?
                                -1 : p.Reviews.Average(r => r.Rating)
                        })
                        .ToListAsync()
                };
            }
        }
    }
}