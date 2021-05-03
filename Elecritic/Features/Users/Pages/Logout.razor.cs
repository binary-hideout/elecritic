using System.Threading.Tasks;

using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Users.Pages {
    public partial class Logout {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private User LoggedInUser { get; set; }

        private string ResultMessage { get; set; }

        public Logout() {
            ResultMessage = "";
        }

        protected override async Task OnInitializedAsync() {
            var authState = await AuthStateTask;
            if (authState.User.Identity.IsAuthenticated) {
                LoggedInUser = new User(authState.User);
            }
            else {
                NavigationManager.NavigateTo("/login");
            }
        }

        public async Task LogOutUser() {
            ResultMessage = "Cerrando sesión...";

            await (AuthStateProvider as AuthenticationService).LogOut();
            NavigationManager.NavigateTo("/");
        }
    }
}
