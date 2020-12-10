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

        public DbSet<Favorite> FavoritesTable { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        /// <summary>
        /// Queries the database for a product of which <see cref="Product.Id"/> matches <paramref name="productId"/>.
        /// </summary>
        /// <param name="productId">ID of the product to get.</param>
        /// <returns>The requested <see cref="Product"/>.</returns>
        public async Task<Product> GetProductAsync(int productId) {
            var product = await ProductsTable
                .Include(p => p.Category)
                .Include(p => p.Company)
                .SingleOrDefaultAsync(p => p.Id == productId);

            // if the specified product wasn't found (is null),
            // return a default instance with Id = 0
            return product ?? new Product();
        }

        /// <summary>
        /// Queries the database for the <see cref="Product.Reviews"/> of <paramref name="product"/>.
        /// </summary>
        /// <param name="product"><see cref="Product"/> of the reviews.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="Review"/>s that correspond to <paramref name="product"/>.</returns>
        public async Task<List<Review>> GetReviewsAsync(Product product) {
            var reviews = await Entry(product)
                .Collection(p => p.Reviews)
                .Query()
                .ToListAsync();

            foreach (var review in reviews) {
                // get review's associated User object
                var user = await Entry(review)
                    .Reference(r => r.User)
                    .Query()
                    .SingleAsync();

                // don't track review's user as it's used as read-only
                Entry(user).State = EntityState.Detached;
            }

            return reviews;
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
                Entry(review.Product).State = EntityState.Unchanged;

                await ReviewsTable.AddAsync(review);
                await SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException) {
                return false;
            }
        }

        /// <summary>
        /// Inserts <paramref name="favorite"/> to the database.
        /// </summary>
        /// <param name="favorite">A new favorite record.</param>
        /// <returns><c>true</c> if the record was successfully created,
        /// otherwise <c>false</c>.</returns>
        public async Task<bool> InsertFavoriteAsync(Favorite favorite) {
            try {
                Entry(favorite.User).State = EntityState.Unchanged;
                Entry(favorite.Product).State = EntityState.Unchanged;

                await FavoritesTable.AddAsync(favorite);
                await SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException) {
                return false;
            }
        }

        /// <summary>
        /// Deletes <paramref name="favorite"/> from the database.
        /// </summary>
        /// <param name="favorite">An existing favorite record.</param>
        /// <returns><c>true</c> if the record was successfully deleted,
        /// otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteFavoriteAsync(Favorite favorite) {
            try {
                Entry(favorite).State = EntityState.Deleted;
                await SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException) {
                return false;
            }
        }

        /// <summary>
        /// Queries the database for a record of <paramref name="productId"/> marked as favorite by <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId"><see cref="User.Id"/> who marked <paramref name="productId"/> as favorite.</param>
        /// <param name="productId"><see cref="Product.Id"/> marked as favorite by <paramref name="userId"/>.</param>
        /// <returns><see cref="Favorite"/> if exists for both <paramref name="userId"/> and <paramref name="productId"/>,
        /// <c>null</c> otherwise.</returns>
        public async Task<Favorite> GetFavoriteAsync(int userId, int productId) {
            return await FavoritesTable
                .SingleOrDefaultAsync(f => f.User.Id == userId && f.Product.Id == productId);
        }
    }
}
