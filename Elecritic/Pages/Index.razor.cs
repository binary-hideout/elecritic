using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Helpers;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Pages {

    public partial class Index {

        [Inject]
        private IndexContext IndexContext { get; set; }

        [Inject]
        private MyFavoritesContext MyFavoritesContext { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        /// <summary>
        /// Customized recommended products for logged in user.
        /// </summary>
        private List<Product> RecommendedProducts { get; set; }

        /// <summary>
        /// Top most popular products.
        /// </summary>
        private List<Product> PopularProducts { get; set; }

        /// <summary>
        /// Top favorite products.
        /// </summary>
        private List<Product> FavoriteProducts { get; set; }

        protected override async Task OnInitializedAsync() {
            PopularProducts = await IndexContext.GetPopularProductsAsync();
            FavoriteProducts = await IndexContext.GetFavoriteProductsAsync();

            var user = (AuthStateProvider as AuthenticationService).LoggedUser;
            // if there's a user logged in
            if (user != null) {
                var userFavoriteProducts = await MyFavoritesContext.GetFavoriteProductsAsync(user.Id);
                var products = await IndexContext.GetAllProductsAsync();

                RecommendedProducts = FuzzyLogic.RecommendProducts(userFavoriteProducts, products, 10);
            }
        }
    }
}