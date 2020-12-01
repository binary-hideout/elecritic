using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {
    public class MyFavoritesContext : MainDbContext {
        public DbSet<Product> ProductsTable { get; set; }

        public DbSet<Favorite> FavoritesTable { get; set; }

        public MyFavoritesContext(DbContextOptions<MyFavoritesContext> options) : base(options) { }

        /// <summary>
        /// Gets all the products in the user´s favorites list
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Product>> GetFavoriteProductsAsync(User user) {
            // IDs of top favorite products
            var productsIds = await FavoritesTable
                //Gather products marked as favorite where the user is the current user
                .Include(f => f.User)
                .Where(f => f.User.Id == user.Id)
                .Include(f => f.Product)             
                // select only the product ID
                .Select(f => f.Product.Id)
               
                .ToArrayAsync();

            return await ProductsTable
                .Where(p => productsIds.Contains(p.Id))
                .Include(p => p.Reviews)
                .ToListAsync();
        }
    }
}
