using System;
using System.Collections.Generic;
using System.Linq;

namespace Elecritic.Models {

    /// <summary>
    /// Class that represents an electronic device.
    /// </summary>
    public class Product {

        /// <summary>
        /// Product identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Overview and some specifications of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Public URL to an image of the product.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Date of product release.
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        public int CategoryId { get; set; }

        /// <summary>
        /// Category of the product.
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Company that made the product.
        /// </summary>
        public virtual Company Company { get; set; }

        /// <summary>
        /// Reviews about the product.
        /// </summary>
        public virtual List<Review> Reviews { get; set; }

        public virtual List<Favorite> Favorites { get; set; }

        /// <summary>
        /// Calculates the average rating of this product <see cref="Reviews"/> property.
        /// </summary>
        /// <returns>Average rating of this product.</returns>
        public double GetAverageRating() {
            return Reviews is null || Reviews.Count == 0 ?
                -1 : Reviews.Average(r => r.Rating);
        }
    }
}
