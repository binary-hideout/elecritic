using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Features.ProductDetails.Commands;
using Elecritic.Features.ProductDetails.Queries;
using Elecritic.Models;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.ProductDetails.Pages {

    /// <summary>
    /// Partial class to implement all needed code of ProductPage razor component
    /// </summary>
    public partial class ProductPage {
        [Parameter]
        public int ProductId { get; set; }

        [Inject]
        private IMediator Mediator { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }
        private User AuthUser { get; set; }

        private ReviewDto ReviewForm { get; set; }

        private Product Product { get; set; }

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
        private bool IsLoading { get; set; }
        private bool IsPublishingReview { get; set; }
        private bool IsChangingFavorite { get; set; }

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
            IsLoading = IsPublishingReview = IsChangingFavorite = IsFavorite = false;
            PublicationMessage = FavoriteChangedMessage = "";
            ReviewForm = new ReviewDto();
        }

        protected override async Task OnInitializedAsync() {
            IsLoading = true;

            Product = (await Mediator.Send(
                    new GetProduct.Query { ProductId = ProductId }))
                .Product;
            IsValidProductId = Product is not null;
            // if the product doesn't exist in database
            if (!IsValidProductId) {
                return;
            }

            var authState = await AuthStateTask;
            // if there's a user logged in
            if (authState.User.Identity.IsAuthenticated) {
                AuthUser = new User(authState.User);
                Favorite = (await Mediator.Send(
                        new GetFavorite.Query {
                            UserId = AuthUser.Id,
                            ProductId = ProductId
                        }))
                    .Favorite;
                // favorite is null if the record doesn't exist,
                // meaning that this product wouldn't be marked as favorite by logged user
                IsFavorite = Favorite != null;
            }

            IsLoading = false;
        }

        /// <summary>
        /// Marks <see cref="Product"/> as favorite of logged in user.
        /// </summary>
        private async Task AddToFavoritesAsync() {
            IsChangingFavorite = true;
            await Task.Delay(1);

            Favorite = new Favorite {
                User = AuthUser,
                Product = Product
            };
            if (await Mediator.Send(new AddFavorite.Command { Favorite = Favorite })) {
                IsFavorite = true;
                FavoriteChangedMessage = $"¡Ahora te gusta {Product.Name}!";
            }
            else {
                FavoriteChangedMessage = $"¡Ahora te gusta {Product.Name}!";
            }

            IsChangingFavorite = false;
        }

        /// <summary>
        /// Removes <see cref="Product"/> from favorites of logged in user.
        /// </summary>
        private async Task RemoveFromFavoritesAsync() {
            IsChangingFavorite = true;

            if (await Mediator.Send(new DeleteFavorite.Command { Favorite = Favorite })) {
                IsFavorite = false;
                FavoriteChangedMessage = $"Ya no te gusta {Product.Name}.";
            }
            else {
                FavoriteChangedMessage = "Lo sentimos, ocurrió un error al quitar de tus favoritos :(";
            }

            IsChangingFavorite = false;
        }

        /// <summary>
        /// Try to publish <see cref="ReviewForm"/> to the database.
        /// </summary>
        private async Task PublishReviewAsync() {
            IsPublishingReview = true;
            // make the button to catch up
            await Task.Delay(1);

            var review = new Review {
                Title = ReviewForm.Title,
                Text = ReviewForm.Text,
                Rating = (byte)ReviewForm.RatingProduct,
                PublishDate = DateTime.Now,
                User = AuthUser,
                ProductId = ProductId
            };

            if (await Mediator.Send(new AddReview.Command { Review = review })) {
                Product.Reviews.Add(review);
                ReviewForm.Clear();
                PublicationMessage = "¡Reseña publicada con éxito!";
            }
            else {
                PublicationMessage = "Lo sentimos, tu reseña no pudo ser publicada :(";
            }

            IsPublishingReview = false;
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