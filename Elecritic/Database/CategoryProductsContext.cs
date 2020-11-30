using System.Linq;
using System.Threading.Tasks;

using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {

    /// <summary>
    /// Database context used in <see cref="Pages.CategoriesCategory"/>.
    /// </summary>
    public class CategoryProductsContext : MainDbContext {

        public DbSet<Category> CategoriesTable { get; set; }

        public CategoryProductsContext(DbContextOptions<CategoryProductsContext> options) : base(options) { }

        /// <summary>
        /// Queries the database for a specified category and populates its list of products.
        /// </summary>
        /// <param name="categoryId">Id of the category.</param>
        /// <param name="batchSize">Number of products to retrieve.</param>
        /// <param name="skipSize">Number of products to skip before retrieving <paramref name="batchSize"/> products.</param>
        /// <returns><see cref="Category"/> with <see cref="Product"/>s of size <paramref name="batchSize"/>.</returns>
        public async Task<Category> GetCategoryWithProductsAsync(int categoryId, int batchSize, int skipSize = 0) {
            var category = await CategoriesTable
                .SingleAsync(c => c.Id == categoryId);

            category.Products = await Entry(category)
                .Collection(c => c.Products)
                .Query()
                .Include(p => p.Reviews)
                .Skip(skipSize)
                .Take(batchSize)
                .ToListAsync();

            return category;
        }
    }
}
