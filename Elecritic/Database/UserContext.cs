using System.Threading.Tasks;

using Elecritic.Models;

using Microsoft.EntityFrameworkCore;

namespace Elecritic.Database {

    /// <summary>
    /// Database context for handling <see cref="User"/> interactions such as login and signup.
    /// See <see cref="Pages.Signup"/> and <see cref="Pages.Login"/>.
    /// </summary>
    public class UserContext : MainDbContext {

        public DbSet<User> UsersTable { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        /// <summary>
        /// Inserts <paramref name="newUser"/> to the database table User.
        /// </summary>
        /// <param name="newUser">new account to be created.</param>
        /// <returns><c>true</c> if the insertion succeeded, <c>false</c> if an exception occurred.</returns>
        public async Task<bool> InsertUserAsync(User newUser) {
            try {
                await UsersTable.AddAsync(newUser);
                await SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException) {
                return false;
            }
        }
    }
}
