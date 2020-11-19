using System.Collections.Generic;

namespace Elecritic.Models {
    /// <summary>
    /// Company or brand of a <see cref="Product"/>.
    ///  To get the actual name, e.g. <c>"Huawei"</c>, access property <see cref="Name"/> or call method <see cref="ToString"/>.
    ///  In the database this class just represents a lookup table.
    /// </summary>
    public class Company {

        /// <summary>
        /// Company identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Actual name of the company.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Products made by a company, e.g. cellphones by Huawei.
        /// </summary>
        public virtual List<Product> Products { get; set; }

        /// <summary>
        /// Get the string representation of the company.
        /// </summary>
        /// <returns><see cref="Name"/></returns>
        public override string ToString() {
            return Name;
        }
    }
}
