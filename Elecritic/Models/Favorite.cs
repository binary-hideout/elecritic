namespace Elecritic.Models {

    /// <summary>
    /// Class that represents the record of a product marked as favorite by a user.
    /// </summary>
    public class Favorite {

        public int Id { get; set; }

        /// <summary>
        /// User who marked the product as favorite.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Product marked as favorite.
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
