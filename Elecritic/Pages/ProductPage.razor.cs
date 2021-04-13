using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        private AuthenticationService AuthenticationService => AuthStateProvider as AuthenticationService;

        private Product Product { get; set; }

        private ReviewDto ReviewModel { get; set; }

        private Favorite Favorite { get; set; }

        /// <summary>
        /// Determines if <see cref="Product"/> is marked as favorite by <see cref="UserService.LoggedUser"/>.
        /// </summary>
        private bool IsFavorite { get; set; }

        /// <summary>
        /// Determines if the passed parameter <see cref="ProductId"/> exists.
        /// It's initialized to <c>true</c> so when the page is loading, the error message isn't immediately showed.
        /// </summary>
        private bool IsValidProductId { get; set; }

        /// <summary>
        /// Message that explains the state of the published <see cref="ReviewModel"/>.
        /// </summary>
        private string PublicationMessage { get; set; }

        /// <summary>
        /// Message that explains if a database call with <see cref="Favorite"/> succeeded or not.
        /// If it's not empty then the buttons will be disabled.
        /// </summary>
        private string FavoriteChangedMessage { get; set; }

        public ProductPage() {
            IsValidProductId = true;
            PublicationMessage = FavoriteChangedMessage = "";
            ReviewModel = new ReviewDto();
        }

        protected override async Task OnInitializedAsync() {
            Product = await ProductContext.GetProductAsync(ProductId);
            IsValidProductId = Product.Id != 0;
            // if the product doesn't exist in database
            if (!IsValidProductId) {
                return;
            }

            Product.Reviews = await ProductContext.GetReviewsAsync(Product);

            var userId = AuthenticationService.LoggedUser.Id;
            // if there's a user logged in
            if (userId != 0) {
                Favorite = await ProductContext.GetFavoriteAsync(userId, ProductId);
                // favorite is null if the record doesn't exist,
                // meaning that this product wouldn't be marked as favorite by logged user
                IsFavorite = Favorite != null;
            }
        }

        /// <summary>
        /// Marks <see cref="Product"/> as favorite of <see cref="UserService.LoggedUser"/>.
        /// </summary>
        private async Task AddToFavoritesAsync() {
            Favorite = new Favorite {
                User = AuthenticationService.LoggedUser,
                Product = Product
            };
            var newFavoriteSucceeded = await ProductContext.InsertFavoriteAsync(Favorite);
            FavoriteChangedMessage = newFavoriteSucceeded ?
                $"¡Ahora te gusta {Product.Name}!" : "Lo sentimos, ocurrió un error al marcar como favorito :(";
        }

        /// <summary>
        /// Removes <see cref="Product"/> from favorites of <see cref="UserService.LoggedUser"/>.
        /// </summary>
        private async Task RemoveFromFavoritesAsync() {
            var removedFavoriteSucceeded = await ProductContext.DeleteFavoriteAsync(Favorite);
            FavoriteChangedMessage = removedFavoriteSucceeded ?
                $"Ya no te gusta {Product.Name}." : "Lo sentimos, ocurrió un error al quitar de tus favoritos :(";
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
                User = AuthenticationService.LoggedUser,
                Product = Product
            };

            var publicationSucceeded = await ProductContext.InsertReviewAsync(review);
            if (publicationSucceeded) {
                PublicationMessage = "¡Reseña publicada con éxito!";
                ReviewModel.Clear();
            }
            else {
                PublicationMessage = "Lo sentimos, tu reseña no pudo ser publicada :(";
            }
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