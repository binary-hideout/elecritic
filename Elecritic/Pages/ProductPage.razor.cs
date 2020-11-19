using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    /// <summary>
    /// Partial class to implement all needed code of ProductPage razor component
    /// </summary>
    public partial class ProductPage {

        [Parameter]
        public string ProductId { get; set; }

        public ReviewDto Review { get; set; }

        /// <summary>
        /// Incomplete void, simply made for future query calls, right now it just calls another void
        /// </summary>
        private void SaveReview() {
            Review.ClearReview();
        }

        [Inject]
        public ReviewService ReviewService { get; set; }

        private Review[] Reviews { get; set; }

        protected override async Task OnInitializedAsync() {
            Review = new ReviewDto();
            Reviews = await ReviewService.GetRandomReviewsAsync(DateTime.Now);
        }

        /// <summary>
        /// Review Model with DataAnnotations to apply on EditForm inside ProductPage.razor page
        /// </summary>
        public class ReviewDto {

            [Required(ErrorMessage = "Este campo no puede estar vacío")]
            public string Text { get; set; }

            [Required]
            [Range(1, 5, ErrorMessage = "Rating sale del rango permitido")]
            public int RatingProduct { get; set; }

            [Required(ErrorMessage = "Este campo no puede estar vacío")]
            public string Recommended { get; set; }

            public ReviewDto() { }

            public ReviewDto(string reviewText, int ratingProduct, string recommended) {
                Text = reviewText;
                RatingProduct = ratingProduct;
                Recommended = recommended;
            }

            /// <summary>
            /// Simply sets everything as empty or as 0
            /// </summary>
            public void ClearReview() {
                Text = "";
                RatingProduct = 0;
                Recommended = "";
            }
        }
    }
}
