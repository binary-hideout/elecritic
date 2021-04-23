using System;
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
    public class List {
        public class ProductDto {
            public int Id { get; set; }
            public string Name { get; set; }
            public int CategoryId { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public double AverageRating { get; set; }

            public ProductDto(Product product) {
                Id = product.Id;
                Name = product.Name;
                CategoryId = product.CategoryId;
                Description = product.Description;
                ImagePath = product.ImagePath;
                AverageRating = product.GetAverageRating();
            }
        }

        public class Response {
            public List<ProductDto> Products { get; set; }
        }

        public class Query : IRequest<Response> {
            public int TopFavorites { get; set; }
            public int TopPopular { get; set; }
            public int CategoryId { get; set; }
            public int SkipNumber { get; set; }
            public int TakeNumber { get; set; }
            public int FavoritesByUserId { get; set; }
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

                if (request.TopFavorites > 0) {
                    _logger.LogInformation("Getting top favorite products.");

                    return new Response {
                        Products = await dbContext.Products
                            .Include(p => p.Reviews)
                            .Join(dbContext.Favorites,
                                p => p.Id,
                                f => f.ProductId,
                                (p, f) => new ProductDto(p))
                            .ToListAsync()
                    };
                }

                else if (request.TopPopular > 0) {
                    _logger.LogInformation("Getting top popular products.");

                    return new Response {
                        Products = await dbContext.Products
                            .OrderByDescending(p => p.Reviews.Count)
                            .Take(request.TopPopular)
                            .Include(p => p.Reviews)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }

                else if (request.CategoryId > 0) {
                    _logger.LogInformation("Getting products by category.");

                    return new Response {
                        Products = await dbContext.Products
                            .Where(p => p.CategoryId == request.CategoryId)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }

                else if (request.FavoritesByUserId > 0) {
                    _logger.LogInformation("Getting user's favorite products.");

                    // IDs of top favorite products
                    var productsIds = await dbContext.Favorites
                        //Gather products marked as favorite where the user is the current user
                        .Where(f => f.UserId == request.FavoritesByUserId)
                        // select only the product ID
                        .Select(f => f.ProductId)
                        .ToArrayAsync();

                    return new Response {
                        Products = await dbContext.Products
                            .Where(p => productsIds.Contains(p.Id))
                            .Include(p => p.Reviews)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }

                else {
                    _logger.LogInformation("Getting all products.");

                    return new Response {
                        Products = await dbContext.Products
                            .Include(p => p.Reviews)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }
            }
        }
    }
}
