using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Features.Products.Modules;
using Elecritic.Features.Products.Queries;
using Elecritic.Models;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Products.Pages {

    public partial class Index {
        [Inject]
        private IMediator Mediator { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        /// <summary>
        /// Customized recommended products for logged in user.
        /// </summary>
        private List<List.ProductDto> RecommendedProducts { get; set; }

        /// <summary>
        /// Top most popular products.
        /// </summary>
        private List<List.ProductDto> PopularProducts { get; set; }

        /// <summary>
        /// Top favorite products.
        /// </summary>
        private List<List.ProductDto> FavoriteProducts { get; set; }

        protected override async Task OnInitializedAsync() {
            PopularProducts = (await Mediator.Send(
                new List.Query { TopPopular = 10 }))
                .Products;
            FavoriteProducts = (await Mediator.Send(
                new List.Query { TopFavorites = 10 }))
                .Products;

            var authState = await AuthStateTask;
            // if there's a user logged in
            if (authState.User.Identity.IsAuthenticated) {
                var user = new User(authState.User);
                var userFavoriteProducts = (await Mediator.Send(
                    new List.Query { FavoritesByUserId = user.Id }))
                    .Products;
                var products = (await Mediator.Send(new List.Query()))
                    .Products;

                RecommendedProducts = FuzzyLogic.RecommendProducts(userFavoriteProducts, products, 10);
            }
        }
    }
}