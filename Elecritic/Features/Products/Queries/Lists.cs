using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.Products.Queries {
    public class Lists {
        public class ProductDto {
            public int Id { get; set; }
            public string Name { get; set; }
            public int CategoryId { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public double AverageRating { get; set; }

            public ProductDto(Product product, double averageRating) {
                Id = product.Id;
                Name = product.Name;
                CategoryId = product.CategoryId;
                Description = product.Description;
                ImagePath = product.ImagePath;
                AverageRating = averageRating;
            }
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
                IQueryable<Product> products;
                if (request.TopFavorites is not null) {
                    _logger.LogInformation("Getting top favorite products.");

                    products = dbContext.Products
                        .Where(p => dbContext.Favorites
                            .GroupBy(f => f.ProductId)
                            .OrderByDescending(g => g.Count())
                            .Select(g => g.Key)
                            .Take((int)request.TopFavorites)
                            .Contains(p.Id));
                }

                else if (request.TopPopular is not null) {
                    _logger.LogInformation("Getting top popular products.");

                    products = dbContext.Products
                        .OrderByDescending(p => p.Reviews.Count)
                        .Take((int)request.TopPopular);
                }

                else if (request.CategoryId is not null) {
                    _logger.LogInformation("Getting products by category.");

                    products = dbContext.Products
                        .Where(p => p.CategoryId == (int)request.CategoryId)
                        .Skip((int)request.SkipNumber)
                        .Take((int)request.TakeNumber);
                }

                else if (request.FavoritesByUserId is not null) {
                    _logger.LogInformation("Getting user's favorite products.");

                    products = dbContext.Products
                        .Join(dbContext.Favorites
                                .Where(f => f.UserId == (int)request.FavoritesByUserId),
                            p => p.Id,
                            f => f.ProductId,
                            (p, _) => p);
                }

                else {
                    _logger.LogInformation("Getting all products.");

                    products = dbContext.Products
                        .Where(p => true);
                }

                return new Response {
                    Products = await products
                        .Select(p => new ProductDto(p,
                            p.Reviews.Count == 0 ?
                                -1 : p.Reviews.Average(r => r.Rating)))
                        .ToListAsync()
                };
            }
        }
    }
}