
using Elecritic.Models;

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
            modelBuilder.Entity<User>(user => {
                user.ToTable(typeof(User).Name);

                user.Property(u => u.Username)
                    .HasMaxLength(20)
                    .IsRequired();
                user.Property(u => u.Password)
                    .HasMaxLength(255)
                    .IsRequired();
                user.Property(u => u.Email)
                    .HasMaxLength(50)
                    .IsRequired();
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

            base.OnModelCreating(modelBuilder);
        }
    }
}