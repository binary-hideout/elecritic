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
        private UserService UserService { get; set; }

        [Inject]
        private MyFavoritesContext MyFavoritesContext { get; set; }

        private List<Product> FavoriteProducts { get; set; }


        protected override async Task OnInitializedAsync() {
            
            var userId = UserService.LoggedUser.Id;
            // if there's a user logged in
            if (userId != 0) {
                FavoriteProducts = await MyFavoritesContext.GetFavoriteProductsAsync(userId);
            }
            
        }
    }
}
