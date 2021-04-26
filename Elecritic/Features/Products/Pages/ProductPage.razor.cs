using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Features.Products.Queries;
using Elecritic.Models;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Products.Pages {

    /// <summary>
    /// Partial class to implement all needed code of ProductPage razor component
    /// </summary>
    public partial class ProductPage {

        [Parameter]
        public int ProductId { get; set; }

        [Inject]
        private IMediator Mediator { get; set; }

        [Inject]
        private ProductContext ProductContext { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private Product Product { get; set; }

        private ReviewDto ReviewForm { get; set; }

        private Favorite Favorite { get; set; }

        /// <summary>
        /// Determines if <see cref="Product"/> is marked as favorite by logged in user.
        /// </summary>
        private bool IsFavorite { get; set; }

        /// <summary>
        /// Determines if the passed parameter <see cref="ProductId"/> exists.
        /// It's initialized to <c>true</c> so when the page is loading, the error message isn't immediately showed.
        /// </summary>
        private bool IsValidProductId { get; set; }

        /// <summary>
        /// Message that explains the state of the published <see cref="ReviewForm"/>.
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
            ReviewForm = new ReviewDto();
        }

        protected override async Task OnInitializedAsync() {
            //Product = await ProductContext.GetProductAsync(ProductId);
            Product = (await Mediator.Send(
                new Details.Query { ProductId = ProductId }))
                .Product;
            IsValidProductId = Product is not null;
            // if the product doesn't exist in database
            if (!IsValidProductId) {
                return;
            }

            var authState = await AuthStateTask;
            // if there's a user logged in
            if (authState.User.Identity.IsAuthenticated) {
                var user = new User(authState.User);
                Favorite = await ProductContext.GetFavoriteAsync(user.Id, ProductId);
                // favorite is null if the record doesn't exist,
                // meaning that this product wouldn't be marked as favorite by logged user
                IsFavorite = Favorite != null;
            }
        }

        /// <summary>
        /// Marks <see cref="Product"/> as favorite of logged in user.
        /// </summary>
        private async Task AddToFavoritesAsync() {
            var authState = await AuthStateTask;
            Favorite = new Favorite {
                User = new User(authState.User),
                Product = Product
            };
            var newFavoriteSucceeded = await ProductContext.InsertFavoriteAsync(Favorite);
            FavoriteChangedMessage = newFavoriteSucceeded ?
                $"¡Ahora te gusta {Product.Name}!" : "Lo sentimos, ocurrió un error al marcar como favorito :(";
        }

        /// <summary>
        /// Removes <see cref="Product"/> from favorites of logged in user.
        /// </summary>
        private async Task RemoveFromFavoritesAsync() {
            var removedFavoriteSucceeded = await ProductContext.DeleteFavoriteAsync(Favorite);
            FavoriteChangedMessage = removedFavoriteSucceeded ?
                $"Ya no te gusta {Product.Name}." : "Lo sentimos, ocurrió un error al quitar de tus favoritos :(";
        }

        /// <summary>
        /// Try to publish <see cref="ReviewForm"/> to the database.
        /// </summary>
        private async Task PublishReview() {
            var authState = await AuthStateTask;

            var review = new Review {
                Title = ReviewForm.Title,
                Text = ReviewForm.Text,
                Rating = (byte)ReviewForm.RatingProduct,
                PublishDate = DateTime.Now,
                User = new User(authState.User),
                Product = Product
            };

            var publicationSucceeded = await ProductContext.InsertReviewAsync(review);
            if (publicationSucceeded) {
                PublicationMessage = "¡Reseña publicada con éxito!";
                ReviewForm.Clear();
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