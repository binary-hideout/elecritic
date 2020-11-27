
using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {

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
            // if a property is not specified in a builder is because Entity Framework
            // automatically recognizes them and doesn't require additional configuration.
            // e.g. a property named "Id" will be a Primary Key in the database table

            modelBuilder.Entity<User>(user => {
                user.ToTable(typeof(User).Name);

                user.Property(u => u.Username)
                    .HasMaxLength(20)
                    .IsRequired();
                user.HasIndex(u => u.Username)
                    .IsUnique();
                user.Property(u => u.Password)
                    .HasMaxLength(255)
                    .IsRequired();
                user.Property(u => u.Email)
                    .HasMaxLength(50)
                    .IsRequired();
                user.HasIndex(u => u.Email)
                    .IsUnique();
                user.Property(u => u.FirstName)
                    .HasMaxLength(25);
                user.Property(u => u.LastName)
                    .HasMaxLength(25);

                user.Ignore(u => u.Reliability);
            });

            modelBuilder.Entity<Review>(review => {
                review.ToTable(typeof(Review).Name);

                review.Property(r => r.Title)
                    .HasMaxLength(50)
                    .IsRequired();
                review.Property(r => r.Text)
                    .HasMaxLength(1200)
                    .IsRequired();
                review.Property(r => r.Rating)
                    .IsRequired();
                review.Property(r => r.PublishDate)
                    .IsRequired();
            });

            modelBuilder.Entity<Product>(product => {
                product.ToTable(typeof(Product).Name);

                product.Property(p => p.Name)
                    .HasMaxLength(60)
                    .IsRequired();
                product.Property(p => p.Description)
                    .HasMaxLength(500);
                product.Property(p => p.ImagePath)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Category>(category => {
                category.ToTable(typeof(Category).Name);

                category.Property(c => c.Name)
                    .HasMaxLength(25)
                    .IsRequired();
            });

            modelBuilder.Entity<Company>(company => {
                company.ToTable(typeof(Company).Name);

                company.Property(c => c.Name)
                    .HasMaxLength(25)
                    .IsRequired();
            });

            modelBuilder.Entity<Favorite>(favorite => {
                favorite.ToTable(typeof(Favorite).Name);
            });

            modelBuilder.Entity<Opinion>(opinion => {
                opinion.ToTable(typeof(Opinion).Name);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}