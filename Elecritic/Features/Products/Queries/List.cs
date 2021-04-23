﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;

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

            public QueryHandler(IDbContextFactory<ElecriticContext> factory) {
                _factory = factory;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken) {
                using var dbContext = _factory.CreateDbContext();

                if (request.TopFavorites > 0) {
                    // IDs of top favorite products
                    int[] productsIds = await dbContext.Favorites
                        .GroupBy(f => f.Product.Id)
                        // count number of records of each product
                        .OrderByDescending(g => g.Count())
                        // select only the product ID
                        .Select(g => g.Key)
                        .Take(request.TopFavorites)
                        .ToArrayAsync();

                    return new Response {
                        Products = await dbContext.Products
                            .Where(p => productsIds.Contains(p.Id))
                            .Include(p => p.Reviews)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }

                else if (request.TopPopular > 0) {
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
                    return new Response {
                        Products = await dbContext.Products
                            .Where(p => p.CategoryId == request.CategoryId)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }

                else if (request.FavoritesByUserId > 0) {
                    // IDs of top favorite products
                    var productsIds = await dbContext.Favorites
                        //Gather products marked as favorite where the user is the current user
                        .Include(f => f.User)
                        .Where(f => f.User.Id == request.FavoritesByUserId)
                        .Include(f => f.Product)
                        // select only the product ID
                        .Select(f => f.Product.Id)
                        .ToArrayAsync();

                    return new Response {
                        Products = await dbContext.Products
                            .Where(p => productsIds.Contains(p.Id))
                            .Include(p => p.Reviews)
                            .Include(p => p.Category)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }

                else {
                    return new Response {
                        Products = await dbContext.Products
                            .Include(p => p.Reviews)
                            .Include(p => p.Category)
                            .Select(p => new ProductDto(p))
                            .ToListAsync()
                    };
                }
            }
        }
    }
}
