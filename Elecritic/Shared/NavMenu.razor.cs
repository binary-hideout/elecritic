using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;


namespace Elecritic.Shared {
    public partial class NavMenu {

        [Parameter]
        public bool UserIsLogged { get; set; }

        private bool collapseNavMenu = true;

        private string UserPage { get; set; }
        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu() {
            collapseNavMenu = !collapseNavMenu;
        }


    }
}
