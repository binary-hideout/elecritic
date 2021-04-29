using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elecritic.Features.Products.Queries {
    public class ListCategories {
        public class CategoryDto {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public int ProductsCount { get; set; }
        }

        public class Response {
            public List<CategoryDto> Categories { get; set; }
        }

        public class Query : IRequest<Response> { }

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
                var random = new Random();

                return new Response {
                    Categories = await dbContext.Categories
                        .Select(c => new CategoryDto {
                            Id = c.Id,
                            Name = c.Name,
                            ProductsCount = c.Products.Count,
                            ImagePath = c.Products
                                .FirstOrDefault()
                                .ImagePath
                        })
                        .ToListAsync()
                };
            }
        }
    }
}
