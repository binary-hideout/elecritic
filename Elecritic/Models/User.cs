using System.Collections.Generic;
using System.Security.Claims;

namespace Elecritic.Models {

    /// <summary>
    /// Class that represents a user account.
    /// </summary>
    public class User {

        /// <summary>
        /// User identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sequence of characters by which the user will be mainly identified to the other users.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User's email, used for authentication.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// First name of the user. Private information.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Hashed password. Useful when trying a login.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Represents a user's score according to how reliable his/her reviews and ratings are.
        /// </summary>
        public int Reliability { get; set; }

        /// <summary>
        /// Reviews made by the user.
        /// </summary>
        public virtual List<Review> Reviews { get; set; }

        /// <summary>
        /// Opinions of reviews from other users.
        /// </summary>
        public virtual List<Opinion> Opinions { get; set; }

        /// <summary>
        /// Favorite products as marked by the user.
        /// </summary>
        public virtual List<Favorite> Favorites { get; set; }

        /// <summary>
        /// Identifier of the user's role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Role object of the user.
        /// </summary>
        public virtual UserRole Role { get; set; }

        public User() { }

        /// <summary>
        /// Creates a new user based on <paramref name="claims"/>.
        /// </summary>
        /// <param name="claims">Claims from a JWT security token.</param>
        public User(ClaimsPrincipal claims) {
            Id = int.Parse(claims.FindFirstValue("NameId"));
            Username = claims.FindFirstValue(nameof(ClaimTypes.Name));
            Email = claims.FindFirstValue(nameof(ClaimTypes.Email));
        }
    }
}
