using System.Collections.Generic;

namespace Elecritic.Models {

    /// <summary>
    /// Class that represents a user account.
    /// </summary>
    public class User {

        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

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
    }
}
