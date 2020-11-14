namespace Elecritic.Models {

    /// <summary>
    /// Class that represents the record of a <see cref="Models.Product"/> marked as favorite by a <see cref="Models.User"/>.
    /// </summary>
    public class Favorite {

        public int Id { get; set; }

        /// <summary>
        /// User who marked the <see cref="Product"/>.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Product marked as favorite by <see cref="User"/>.
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
