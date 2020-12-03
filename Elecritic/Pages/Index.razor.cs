using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Helpers;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class Index {
        [Inject]
        private IndexContext IndexContext { get; set; }

        [Inject]
        private MyFavoritesContext MyFavoritesContext { get; set; }

        [Inject]
        private UserService UserService { get; set; }

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

            int userId = UserService.LoggedUser.Id;
            if (userId != 0) {
                var userFavoriteProducts = await MyFavoritesContext.GetFavoriteProductsAsync(userId);
                var products = await IndexContext.Get3StarsProductsAsync();

                RecommendedProducts = FuzzyLogic.GetRecommended(userFavoriteProducts, products, 10);
            }
        }
    }
}