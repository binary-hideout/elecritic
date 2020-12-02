using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class Index {
        [Inject]
        private IndexContext IndexContext { get; set; }

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
        }
    }
}
