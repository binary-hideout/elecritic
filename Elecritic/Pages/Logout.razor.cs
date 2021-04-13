using System.Threading.Tasks;

using Elecritic.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Pages {
    public partial class Logout {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        private string ResultMessage { get; set; }

        public Logout() {
            ResultMessage = "";
        }

        public async Task LogOutUser() {
            ResultMessage = "Cerrando sesión...";

            await (AuthStateProvider as AuthenticationService).LogOut();
            NavigationManager.NavigateTo("/");
        }
    }
}
