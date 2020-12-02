using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Helpers;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class Logout {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserContext UserContext { get; set; }

        [Inject]
        public UserService UserService { get; set; }

        private string ResultMessage { get; set; } = "";

        public async Task LogOutUser() {

            ResultMessage = "Cerrando sesión...";

            UserService.LogOff();

            NavigationManager.NavigateTo("/");

            
        }
    }
}
