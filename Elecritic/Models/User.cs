namespace Elecritic.Models {

    public class User {

        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        /// Represents a user's score according to how reliable his/her reviews and ratings are.
        /// </summary>
        public int Reliability { get; set; }
    }
}
