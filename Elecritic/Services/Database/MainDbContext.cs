
using Microsoft.EntityFrameworkCore;

namespace Elecritic.Services.Database {

    /// <summary>
    /// Application database context. Builds every necessary model class to a MySQL table.
    /// This context does not contain any call to the database, and it is not injected as a service (is not usable).
    /// The usable contexts inherit from this class.
    /// Its purpose is to use the Code First feature of Entity Framework.
    /// </summary>
    public class MainDbContext : DbContext {

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        protected MainDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
        }
    }
}