using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elecritic.Shared {
    public partial class NavMenu2 {
        private bool collapseNavMenu = true;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu() {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
