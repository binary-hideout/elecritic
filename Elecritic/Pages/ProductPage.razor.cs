using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class ProductPage {

        [Parameter]
        public string ProductId { get; set; }

        public ReviewModel review { get; set; }

        protected override void OnInitialized() {
            review = new ReviewModel();
        }

        private void SaveReview() {
            //reviewService.SaveReview(review);
            review.ClearReview();
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
