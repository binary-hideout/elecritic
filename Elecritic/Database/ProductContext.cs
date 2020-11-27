using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {

    /// <summary>
    /// Database context for <see cref="Pages.ProductPage"/>.
    /// Contains methods to get a <see cref="Product"/> and its <see cref="Product.Reviews"/>, and to add a new <see cref="Review"/>.
    /// </summary>
    public class ProductContext : MainDbContext {

        public DbSet<Product> ProductsTable { get; set; }

        public DbSet<Review> ReviewsTable { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        /// <summary>
        /// Queries the database for a product of which <see cref="Product.Id"/> matches <paramref name="productId"/>.
        /// </summary>
        /// <param name="productId">ID of the product to get.</param>
        /// <returns>The requested <see cref="Product"/>.</returns>
        public async Task<Product> GetProductAsync(int productId) {
            return await ProductsTable
                .Include(p => p.Category)
                .Include(p => p.Company)
                .SingleAsync(p => p.Id == productId);
        }

        /// <summary>
        /// Queries the database for the <see cref="Product.Reviews"/> of <paramref name="product"/>.
        /// </summary>
        /// <param name="product"><see cref="Product"/> of the reviews.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="Review"/>s that correspond to <paramref name="product"/>.</returns>
        public async Task<List<Review>> GetProductReviewsAsync(Product product) {
            return await Entry(product)
                .Collection(p => p.Reviews)
                .Query()
                .ToListAsync();
        }

        /// <summary>
        /// Inserts <paramref name="review"/> to the database.
        /// </summary>
        /// <param name="review">a new <see cref="Review"/> of a product.</param>
        /// <returns><c>true</c> if <paramref name="review"/> is successfully added to the database,
        /// <c>false</c> if an exception occurred.</returns>
        public async Task<bool> InsertReviewAsync(Review review) {
            try {
                Entry(review.User).State = EntityState.Unchanged;

                await ReviewsTable.AddAsync(review);
                await SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException) {
                return false;
            }
        }
    }
}
