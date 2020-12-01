using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace Elecritic.Pages {
    public partial class MyFavorites {

        [Inject]
        public ProductService ProductService { get; set; }
        [Inject]
        private UserService UserService { get; set; }

        [Inject]
        private MyFavoritesContext MyFavoritesContext { get; set; }

        private List<Product> FavoriteProducts { get; set; }

        private Product[] Products { get; set; }

        protected override async Task OnInitializedAsync() {
            Products = await ProductService.GetRandomProductsAsync(DateTime.Now);
            var user = UserService.LoggedUser;
            // if there's a user logged in
            if (user != null) {
                FavoriteProducts = await MyFavoritesContext.GetFavoriteProductsAsync(user);
            }
            
        }
    }
}
