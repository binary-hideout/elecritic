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
        /// If it doesn't exist, a default instance will be returned, with Id = 0.
        /// </summary>
        /// <param name="categoryId">Id of the category.</param>
        /// <param name="batchSize">Number of products to retrieve.</param>
        /// <param name="skipSize">Number of products to skip before retrieving <paramref name="batchSize"/> products.</param>
        /// <returns><see cref="Category"/> including <see cref="Category.Products"/> with count <paramref name="batchSize"/> if exists,
        /// else a default instance with <see cref="Category.Id"/> equal to <c>0</c>.
        /// </returns>
        public async Task<Category> GetCategoryWithProductsAsync(int categoryId, int batchSize, int skipSize = 0) {
            var category = await CategoriesTable
                .SingleOrDefaultAsync(c => c.Id == categoryId);
            // if there's no category that matches specified Id
            if (category is null) {
                // return a default category, Id = 0
                return new Category();
            }

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