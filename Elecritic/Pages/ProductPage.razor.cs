using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Elecritic.Models;
using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class ProductPage {

        [Parameter]
        public string ProductId { get; set; }

        public ReviewModel Review { get; set; }

        protected override void OnInitialized() {
            Review = new ReviewModel();
        }

        private void SaveReview() {
            //reviewService.SaveReview(review);
            Review.ClearReview();
        }

        //ReviewService
        [Inject]
        public ReviewService ReviewService { get; set; }

        private Review[] Reviews { get; set; }

        protected override async Task OnInitializedAsync() {
            Reviews = await ReviewService.GetRandomReviewsAsync(DateTime.Now);
        }
    }

    public class ReviewModel {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        public string Text { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating sale del rango permitido")]
        public float RatingProduct { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        public string Recommended { get; set; }


        public ReviewModel() {

        }

        public ReviewModel(string authorId, string reviewText, int ratingProduct, string recommended) {
            UserId = authorId;
            Text = reviewText;
            RatingProduct = ratingProduct;
            Recommended = recommended;

        }

        public void ClearReview() {
            UserId = "";
            Text = "";
            RatingProduct = 0;
            Recommended = "";


        }
    }
}
