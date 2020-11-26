using System.Threading.Tasks;
using System.Linq;

using Elecritic.Models;

using Microsoft.EntityFrameworkCore;
using Elecritic.Pages;

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

        /// <summary>
        /// Queries the database for the password of a <see cref="User"/> whose email matches <paramref name="userEmail"/>.
        /// </summary>
        /// <param name="userEmail">email provided in <see cref="Pages.Login"/>.</param>
        /// <returns>hashed password if the user exists, otherwise an empty string.</returns>
        public async Task<string> GetHashedPasswordAsync(string userEmail) {
            var user = await UsersTable
                .Select(u => new Login.UserDto {
                    Email = u.Email,
                    Password = u.Password
                })
                .SingleOrDefaultAsync(u => u.Email == userEmail);

            return user is null ? string.Empty : user.Password;
        }
    }
}
