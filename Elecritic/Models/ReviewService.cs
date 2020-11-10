using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elecritic.Models {
    public class ReviewService {
        private static readonly string[] FakeReviews = new[] {
            "good one, mine is 3 years old and still works fine", "Great option considering its price",
            "There are better options, but this one is not a bad choice",
            "After a year of use you could face some issues with the camera, everything else works fine",
            "I have never had an issue with it, its the best one for the price range no doubt"
        };
        public Task<Review[]> GetRandomReviewsAsync(DateTime startDate) {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 7).Select(index => new Review {
                Id = 1,
                UserId = 1,
                Title = "Average Review, mostly positive",
                Text = FakeReviews[rng.Next(FakeReviews.Length)],
                Rating = 4,
                PublishDate = startDate.AddDays(index),
            }).ToArray());
        }
    }
}
