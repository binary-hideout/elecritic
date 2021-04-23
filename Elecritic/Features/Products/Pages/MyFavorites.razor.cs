using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Features.Products.Queries;
using Elecritic.Models;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Products.Pages {
    public partial class MyFavorites {

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IMediator Mediator { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private List<List.ProductDto> FavoriteProducts { get; set; }

        protected override async Task OnInitializedAsync() {
            var authState = await AuthStateTask;
            // if there's a logged in user
            if (authState.User.Identity.IsAuthenticated) {
                // load his/her favorite products
                var user = new User(authState.User);
                FavoriteProducts = (await Mediator.Send(
                    new List.Query { FavoritesByUserId = user.Id }))
                    .Products;
            }
            else {
                // redirect to Login page
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}