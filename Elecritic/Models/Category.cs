namespace Elecritic.Models {
    /// <summary>
    /// Category of a <see cref="Product"/>.
    ///  To get the actual name, e.g. <c>"Cellphone"</c>, access property <see cref="Name"/> or call method <see cref="ToString"/>.
    ///  In the database this class just represents a lookup table.
    /// </summary>
    public class Category {

        public int Id { get; set; }

        /// <summary>
        /// Actual name of the category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get the string representation of the category.
        /// </summary>
        /// <returns><see cref="Name"/></returns>
        public override string ToString() {
            return Name;
        }
    }
}
