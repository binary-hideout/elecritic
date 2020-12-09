
using Microsoft.AspNetCore.Components;


namespace Elecritic.Shared {
    public partial class NavMenu {

        [Parameter]
        public bool UserIsLogged { get; set; }

        private bool CollapseNavMenu { get; set; }

        private string NavMenuCssClass => CollapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu() {
            CollapseNavMenu = !CollapseNavMenu;
        }
    }
}