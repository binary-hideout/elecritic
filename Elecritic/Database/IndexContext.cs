using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {

    /// <summary>
    /// Database context for <see cref="Pages.Index"/> page.
    /// </summary>
    public class IndexContext : MainDbContext {

        public DbSet<Product> ProductsTable { get; set; }

        public DbSet<Favorite> FavoritesTable { get; set; }

        public DbSet<Review> ReviewsTable { get; set; }

        public IndexContext(DbContextOptions<IndexContext> options) : base(options) { }

        /// <summary>
        /// Queries the database for a maximum of <paramref name="number"/> <see cref="Product"/>s
        /// sorted descending by their <see cref="Product.Reviews"/>.
        /// </summary>
        /// <param name="number">How many products to get.</param>
        /// <returns>The top <paramref name="number"/> most popular products.</returns>
        public async Task<List<Product>> GetPopularProductsAsync(int number = 10) {
            return await ProductsTable
                .OrderByDescending(p => p.Reviews.Count)
                .Take(number)
                .Include(p => p.Reviews)
                .ToListAsync();
        }

        /// <summary>
        /// Queries the database for a maximum of <paramref name="number"/> <see cref="Product"/>s
        /// sorted descending by the times that they have been marked as <see cref="Favorite"/>.
        /// </summary>
        /// <param name="number">How many products to get.</param>
        /// <returns>The top <paramref name="number"/> most favorite products.</returns>
        public async Task<List<Product>> GetFavoriteProductsAsync(int number = 10) {
            // IDs of top favorite products
            int[] productsIds = await FavoritesTable
                .GroupBy(f => f.Product.Id)
                // count number of records of each product
                .OrderByDescending(g => g.Count())
                // select only the product ID
                .Select(g => g.Key)
                .Take(number)
                .ToArrayAsync();

            return await ProductsTable
                .Where(p => productsIds.Contains(p.Id))
                .Include(p => p.Reviews)
                .ToListAsync();
        }

        /// <summary>
        /// Queries the database for all the available products.
        /// </summary>
        /// <returns>A list of all <see cref="Product"/>s.</returns>
        public async Task<List<Product>> GetAllProductsAsync() {
            return await ProductsTable
                .Include(p => p.Reviews)
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}