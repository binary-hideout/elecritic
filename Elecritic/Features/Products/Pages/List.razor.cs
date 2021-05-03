using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Models;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace Elecritic.Features.Products.Pages {
    public partial class List : IDisposable {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IMediator Mediator { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private bool IsValidQuery { get; set; }
        private bool IsLoading { get; set; }

        private string InvalidMessage { get; set; }
        private string Title { get; set; }

        private Queries.List.Query Query { get; set; }
        private List<Queries.List.ProductDto> Products { get; set; }

        public List() {
            IsValidQuery = true;
            IsLoading = false;
            InvalidMessage = Title = "";
            Products = new List<Queries.List.ProductDto>();
        }

        protected override async Task OnInitializedAsync() {
            await ParseQueryString();
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        /// <summary>
        /// Handles <see cref="NavigationManager.LocationChanged"/> event.
        /// </summary>
        private async void OnLocationChanged(object sender, LocationChangedEventArgs e) {
            await InvokeAsync(async () => {
                await ParseQueryString();
                StateHasChanged();
            });
        }

        private async Task ParseQueryString() {
            IsLoading = true;

            Query = new Queries.List.Query();
            Title = "";
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("categoryid", out var categoryId)) {
                Query.CategoryId = int.Parse(categoryId);
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("category", out var categoryName)) {
                Title = categoryName.ToString();
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("myfavs", out var myFavs) && bool.Parse(myFavs)) {
                var authState = await AuthStateTask;
                if (authState.User.Identity.IsAuthenticated) {
                    var user = new User(authState.User);
                    Query.FavoritesByUserId = user.Id;
                    Title = $"Mis {Title.ToLower()} favoritos";
                }
                else {
                    IsValidQuery = false;
                    InvalidMessage = "No has iniciado sesión.";
                }
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("skip", out var skipNumber)) {
                Query.SkipNumber = int.Parse(skipNumber);
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("take", out var takeNumber)) {
                Query.TakeNumber = int.Parse(takeNumber);
            }

            Products = (await Mediator.Send(Query)).Products;

            IsLoading = false;
        }

        public void Dispose() {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}