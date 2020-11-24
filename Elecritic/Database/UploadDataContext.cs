using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {

    /// <summary>
    /// Database calls to allow administrators to upload data (new categories and new products).
    /// See <see cref="Pages.UploadFiles"/>.
    /// </summary>
    public class UploadDataContext : MainDbContext {

        public DbSet<Product> ProductsTable { get; set; }

        public DbSet<Company> CompaniesTable { get; set; }

        public DbSet<Category> CategoriesTable { get; set; }

        public UploadDataContext(DbContextOptions<UploadDataContext> options) : base(options) { }

        public async Task InsertCategoryAsync(Category category) {
            await CategoriesTable.AddAsync(category);
            await SaveChangesAsync();
        }

        public async Task InsertCompanyAsync(Company company) {
            await CompaniesTable.AddAsync(company);
            await SaveChangesAsync();
        }

        public async Task InsertProductAsync(Product product) {
            await ProductsTable.AddAsync(product);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Returns an array of categories that are registered in the database,
        /// without their <see cref="Category.Products"/>.
        /// </summary>
        public async Task<Category[]> GetCategoriesAsync() {
            return await CategoriesTable.ToArrayAsync();
        }

        /// <summary>
        /// Returns a list of the current companies  that are registered in the database,
        /// without their <see cref="Company.Products"/>.
        /// </summary>
        public async Task<List<Company>> GetCompaniesAsync() {
            return await CompaniesTable.ToListAsync();
        }
    }
}
