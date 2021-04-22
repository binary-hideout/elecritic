using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Pages {
    public partial class MyFavorites {

        [Inject]
        private MyFavoritesContext MyFavoritesContext { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private List<Product> FavoriteProducts { get; set; }

        protected override async Task OnInitializedAsync() {
            var authState = await AuthStateTask;
            // if there's a logged in user
            if (authState.User.Identity.IsAuthenticated) {
                // load his/her favorite products
                var user = new User(authState.User);
                FavoriteProducts = await MyFavoritesContext.GetFavoriteProductsAsync(user.Id);
            }
            else {
                // redirect to Login page
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}