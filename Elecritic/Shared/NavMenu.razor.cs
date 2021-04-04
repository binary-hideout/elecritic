namespace Elecritic.Shared {
    public partial class NavMenu {

        private bool CollapseNavMenu { get; set; } = true;

        private string NavMenuCssClass => CollapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu() {
            CollapseNavMenu = !CollapseNavMenu;
        }
    }
}