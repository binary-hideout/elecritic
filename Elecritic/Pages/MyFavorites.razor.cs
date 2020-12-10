using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class MyFavorites {

        [Inject]
        private UserService UserService { get; set; }

        [Inject]
        private MyFavoritesContext MyFavoritesContext { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private List<Product> FavoriteProducts { get; set; }

        protected override async Task OnInitializedAsync() {
            var userId = UserService.LoggedUser.Id;
            // if no user logged in
            if (userId == 0) {
                // redirect to Login page
                NavigationManager.NavigateTo("/login");
            }
            // if there's a user logged in
            else {
                // load his/her favorite products
                FavoriteProducts = await MyFavoritesContext.GetFavoriteProductsAsync(userId);
            }
        }
    }
}