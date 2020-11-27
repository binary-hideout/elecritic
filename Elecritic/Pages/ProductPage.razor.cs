using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    /// <summary>
    /// Partial class to implement all needed code of ProductPage razor component
    /// </summary>
    public partial class ProductPage {

        [Parameter]
        public int ProductId { get; set; }

        [Inject]
        private ProductContext ProductContext { get; set; }

        [Inject]
        private UserService UserService { get; set; }

        private string PublicationMessage { get; set; } = "";

        private Product Product { get; set; }

        private ReviewDto ReviewModel { get; set; } = new ReviewDto();

        protected override async Task OnInitializedAsync() {
            Product = await ProductContext.GetProductAsync(ProductId);
            Product.Reviews = await ProductContext.GetProductReviewsAsync(Product);
        }

        /// <summary>
        /// Try to publish <see cref="ReviewModel"/> to the database.
        /// </summary>
        private async Task PublishReview() {
            var review = new Review {
                Title = ReviewModel.Title,
                Text = ReviewModel.Text,
                Rating = (byte)ReviewModel.RatingProduct,
                PublishDate = DateTime.Now,
                User = UserService.LoggedUser,
                Product = Product
            };

            bool publicationSucceeded = await ProductContext.InsertReviewAsync(review);
            if (publicationSucceeded) {
                PublicationMessage = "¡Reseña publicada con éxito!";
            }
            else {
                PublicationMessage = "Lo sentimos, tu reseña no pudo ser publicada :(";
            }

            ReviewModel.Clear();
        }

        /// <summary>
        /// Review Model with DataAnnotations to apply on EditForm inside ProductPage.razor page
        /// </summary>
        public class ReviewDto {

            [Required(ErrorMessage = "Este campo no puede estar vacío")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Este campo no puede estar vacío")]
            public string Text { get; set; }

            [Required]
            [Range(1, 5, ErrorMessage = "Rating sale del rango permitido")]
            public int RatingProduct { get; set; }

            [Required(ErrorMessage = "Este campo no puede estar vacío")]
            public string Recommended { get; set; }

            public ReviewDto() { }

            /// <summary>
            /// Simply sets everything as empty or as 0
            /// </summary>
            public void Clear() {
                Title = "";
                Text = "";
                RatingProduct = 0;
                Recommended = "";
            }
        }
    }
}
