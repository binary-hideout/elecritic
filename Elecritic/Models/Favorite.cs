namespace Elecritic.Models {
    /// <summary>
    /// Class that represents the record of a <see cref="Models.Product"/> marked as favorite by a <see cref="Models.User"/>.
    /// </summary>
    public class Favorite {
        public int Id { get; set; }

        public int UserId { get; set; }
        /// <summary>
        /// User who marked <see cref="Product"/> as favorite.
        /// </summary>
        public virtual User User { get; set; }

        public int ProductId { get; set; }
        /// <summary>
        /// Product marked as favorite by <see cref="User"/>.
        /// </summary>
        public virtual Product Product { get; set; }
    }
}