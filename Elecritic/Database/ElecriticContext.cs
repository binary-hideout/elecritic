using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {
    /// <summary>
    /// Application database context. Builds every necessary model class to a MySQL table.
    /// Its purpose is to use the Code First feature of Entity Framework.
    /// </summary>
    public class ElecriticContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Opinion> Opinions { get; set; }

        public ElecriticContext(DbContextOptions<ElecriticContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // if a property is not specified in a builder is because Entity Framework
            // automatically recognizes them and doesn't require additional configuration.
            // e.g. a property named "Id" will be a Primary Key in the database table

            modelBuilder.Entity<User>(user => {
                user.ToTable(nameof(User));

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

                user.HasOne(u => u.Role)
                        .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId);

                user.Ignore(u => u.Reliability);
            });

            modelBuilder.Entity<UserRole>(userRole => {
                userRole.ToTable(nameof(UserRole));

                userRole.Property(r => r.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            modelBuilder.Entity<Review>(review => {
                review.ToTable(nameof(Review));

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
                product.ToTable(nameof(Product));

                product.Property(p => p.Name)
                    .HasMaxLength(60)
                    .IsRequired();
                product.Property(p => p.Description)
                    .HasMaxLength(500);
                product.Property(p => p.ImagePath)
                    .HasMaxLength(100);
                product.HasOne(p => p.Category)
                        .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Category>(category => {
                category.ToTable(nameof(Category));

                category.Property(c => c.Name)
                    .HasMaxLength(25)
                    .IsRequired();
            });

            modelBuilder.Entity<Company>(company => {
                company.ToTable(nameof(Company));

                company.Property(c => c.Name)
                    .HasMaxLength(25)
                    .IsRequired();
            });

            modelBuilder.Entity<Favorite>(favorite => {
                favorite.ToTable(nameof(Favorite));

                favorite.HasOne(f => f.User)
                        .WithMany(u => u.Favorites)
                    .HasForeignKey(f => f.UserId);
            });

            modelBuilder.Entity<Opinion>(opinion => {
                opinion.ToTable(nameof(Opinion));
            });
        }
    }
}