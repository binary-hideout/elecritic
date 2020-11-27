
using Elecritic.Models;

namespace Elecritic.Services {

    /// <summary>
    /// Service that handles the logged in user, see <see cref="LoggedUser"/>.
    /// </summary>
    public class UserService {

        /// <summary>
        /// Current logged in user.
        /// If no user is logged in, the property holds an empty instance, and the <see cref="User.Id"/> is <c>0</c>.
        /// </summary>
        public User LoggedUser { get; set; } = new User();

        /// <summary>
        /// Sets <see cref="LoggedUser"/> to be <paramref name="user"/>.
        /// </summary>
        /// <param name="user"><see cref="User"/> who is logging in.</param>
        public void LogIn(User user) {
            LoggedUser = user;
        }

        /// <summary>
        /// Sets <see cref="LoggedUser"/> to be an empty instance of <see cref="User"/>.
        /// </summary>
        public void LogOff() {
            LoggedUser = new User();
        }
    }
}
