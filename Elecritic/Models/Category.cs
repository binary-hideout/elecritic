namespace Elecritic.Models {
    /// <summary>
    /// Category of a <see cref="Product"/>.
    ///  To get the actual name, e.g. <c>"Cellphone"</c>, access <see cref="Name"/>
    /// </summary>
    public class Category {

        public int Id { get; set; }

        /// <summary>
        /// Actual name of this category.
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
