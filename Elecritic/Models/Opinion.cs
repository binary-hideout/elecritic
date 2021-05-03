namespace Elecritic.Models {

    /// <summary>
    /// Opinion that a <see cref="Models.User"/> can have about a <see cref="Models.Review"/> from another user:
    /// whether he finds it helpful or not. See <see cref="IsHelpful"/>.
    /// This allows to estimate the <see cref="User.Reliability"/>.
    /// </summary>
    public class Opinion {

        /// <summary>
        /// Opinion identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Actual opinion. Determines if a review was helpful or not, according to <see cref="User"/>.
        /// </summary>
        public bool IsHelpful { get; set; }

        /// <summary>
        /// User who made the opinion.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Review that gets the opinion.
        /// </summary>
        public virtual Review Review { get; set; }
    }
}
