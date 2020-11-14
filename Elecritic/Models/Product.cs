using System;
using System.Collections.Generic;

namespace Elecritic.Models {

    /// <summary>
    /// Class that represents an electronic device.
    /// </summary>
    public class Product {

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Overview and some specifications of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Public URL to an image of the product.
        /// </summary>
        public string ImagePath { get; set; }

        public DateTime ReleaseDate { get; set; }

        public virtual Category Category { get; set; }

        public virtual Company Company { get; set; }

        /// <summary>
        /// Reviews about the product.
        /// </summary>
        public virtual List<Review> Reviews { get; set; }
    }
}
