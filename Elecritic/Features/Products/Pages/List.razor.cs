using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Models;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;

namespace Elecritic.Features.Products.Pages {
    public partial class List {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IMediator Mediator { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        /// <summary>
        /// Determines if the passed parameter <see cref="CategoryId"/> exists.
        /// It's initialized to <c>true</c> so when the page is loading, the error message isn't immediately showed.
        /// </summary>
        private bool IsValidQuery { get; set; }
        private string InvalidMessage { get; set; }

        private List<Queries.List.ProductDto> Products { get; set; }

        private string Title { get; set; }

        private bool IsLoading { get; set; }

        public List() {
            IsValidQuery = true;
            InvalidMessage = Title = "";
            IsLoading = false;
        }

        /// <summary>
        /// Based on the Category received as parameter this method will get the corresponding products.
        /// </summary>
        protected override async Task OnInitializedAsync() {
            IsLoading = true;

            var query = new Queries.List.Query();

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("categoryid", out var categoryId)) {
                query.CategoryId = int.Parse(categoryId);
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("category", out var categoryName)) {
                Title = categoryName.ToString();
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("myfavs", out var myFavs) && bool.Parse(myFavs)) {
                var authState = await AuthStateTask;
                if (authState.User.Identity.IsAuthenticated) {
                    var user = new User(authState.User);
                    query.FavoritesByUserId = user.Id;
                    Title = $"Mis {Title.ToLower()} favoritos";
                }
                else {
                    IsValidQuery = false;
                    InvalidMessage = "No has iniciado sesión.";
                }
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("skip", out var skipNumber)) {
                query.SkipNumber = int.Parse(skipNumber);
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("take", out var takeNumber)) {
                query.TakeNumber = int.Parse(takeNumber);
            }

            Products = (await Mediator.Send(query))
                .Products;

            IsLoading = false;
        }
    }
}