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

        private Product Product { get; set; }

        private ReviewDto ReviewModel { get; set; } = new ReviewDto();

        private Favorite Favorite { get; set; }

        /// <summary>
        /// Determines if <see cref="Product"/> is marked as favorite by <see cref="UserService.LoggedUser"/>.
        /// </summary>
        private bool IsFavorite { get; set; }

        private string PublicationMessage { get; set; } = "";

        /// <summary>
        /// Message that explains if a database call with <see cref="Favorite"/> succeeded or not.
        /// If it's not <see cref="string.Empty"/> then the buttons will be disabled.
        /// </summary>
        private string FavoriteChangedMessage { get; set; } = "";

        protected override async Task OnInitializedAsync() {
            Product = await ProductContext.GetProductAsync(ProductId);
            Product.Reviews = await ProductContext.GetProductReviewsAsync(Product);

            int userId = UserService.LoggedUser.Id;
            // if there's a user logged in
            if (userId != 0) {
                Favorite = await ProductContext.GetFavoriteAsync(userId, ProductId);
                // favorite is null if the record doesn't exist,
                // meaning that this product wouldn't be marked as favorite by logged user
                IsFavorite = !(Favorite is null);
            }
        }

        /// <summary>
        /// Marks <see cref="Product"/> as favorite of <see cref="UserService.LoggedUser"/>.
        /// </summary>
        private async Task AddToFavoritesAsync() {
            Favorite = new Favorite {
                User = UserService.LoggedUser,
                Product = Product
            };
            bool newFavoriteSucceeded = await ProductContext.InsertFavoriteAsync(Favorite);
            if (newFavoriteSucceeded) {
                FavoriteChangedMessage = $"¡Ahora te gusta {Product.Name}!";
            }
            else {
                FavoriteChangedMessage = "Lo sentimos, ocurrió un error al marcar como favorito :(";
            }
        }

        /// <summary>
        /// Removes <see cref="Product"/> from favorites of <see cref="UserService.LoggedUser"/>.
        /// </summary>
        private async Task RemoveFromFavoritesAsync() {
            bool removedFavoriteSucceeded = await ProductContext.DeleteFavoriteAsync(Favorite);
            if (removedFavoriteSucceeded) {
                FavoriteChangedMessage = $"Ya no te gusta {Product.Name}.";
            }
            else {
                FavoriteChangedMessage = "Lo sentimos, ocurrió un error al quitar de tus favoritos :(";
            }
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
