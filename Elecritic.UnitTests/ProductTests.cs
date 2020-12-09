using System.Collections.Generic;
using System.Linq;

using Elecritic.Models;

using Xunit;

namespace Elecritic.UnitTests {

    /// <summary>
    /// Unit tests for <see cref="Product"/> model class.
    /// </summary>
    public class ProductTests {

        /// <summary>
        /// Creates a list of <paramref name="count"/> fake reviews.
        /// </summary>
        /// <param name="count">Number of reviews to create.</param>
        /// <param name="fakeRating">Rating of each review.</param>
        private List<Review> CreateFakeReviews(int count, byte fakeRating) {
            var fakeReview = new Review {
                Rating = fakeRating
            };
            return count < 0 ?
                null : Enumerable.Repeat(fakeReview, count).ToList();
        }

        /// <summary>
        /// Tests that method <see cref="Product.GetAverageRating"/> returns <c>-1</c>
        /// when <see cref="Product.Reviews"/> is <c>null</c> or an empty list.
        /// </summary>
        /// <param name="count">Count of <see cref="Product.Reviews"/>, it's -1 for null and 0 for empty.</param>
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetAverageRating_WhenReviewsIsNullOrEmpty_ReturnsMinusOne(int count) {
            var emptyProduct = new Product {
                Reviews = CreateFakeReviews(count, 0)
            };

            double actual = emptyProduct.GetAverageRating();

            Assert.Equal(-1, actual);
        }

        /// <summary>
        /// Generates parameters for <see cref="GetAverageRating_WhenReviewsHasItems_ReturnsAverageRating(int, byte)"/>.
        /// </summary>
        public static IEnumerable<object[]> CountsRatings =>
            Enumerable.Range(1, 5).Select(i => new object[] { i, i }).ToList();

        /// <summary>
        /// Tests that method <see cref="Product.GetAverageRating"/> returns the correct calculation
        /// when a product has reviews.
        /// </summary>
        /// <param name="count">Count of <see cref="Product.Reviews"/>.</param>
        /// <param name="rating">Rating of each review in <see cref="Product.Reviews"/>.</param>
        [Theory]
        [MemberData(nameof(CountsRatings))]
        public void GetAverageRating_WhenReviewsHasItems_ReturnsAverageRating(int count, byte rating) {
            var productWithReviews = new Product {
                Reviews = CreateFakeReviews(count, rating)
            };

            double actual = productWithReviews.GetAverageRating();

            Assert.Equal(rating, actual);
        }
    }
}