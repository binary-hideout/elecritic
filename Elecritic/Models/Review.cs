using System;
using System.Collections.Generic;

namespace Elecritic.Models {

    /// <summary>
    /// Class that represents a review of an electronic device.
    /// </summary>
    public class Review {

        public int Id { get; set; }

        /// <summary>
        /// Short title of the review. Should be like a summary of <see cref="Text"/>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Actual written review.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Rating on a five-star range.
        /// </summary>
        public byte Rating { get; set; }

        /// <summary>
        /// Date published of the review.
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// User who made the review.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Product that gets the review.
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Opinions from users about the review.
        /// </summary>
        public virtual List<Opinion> Opinions { get; set; }
    }
}
