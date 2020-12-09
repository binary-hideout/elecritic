using System.Linq;

using Bunit;

using Elecritic.Pages;
using Elecritic.Shared;
using Elecritic.UnitTests.Fakes;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Elecritic.UnitTests {

    /// <summary>
    /// Unit tests for Razor component <see cref="NavMenu"/>.
    /// </summary>
    public class NavMenuTests : TestContext {

        /// <summary>
        /// Tests that the button to navigate to <see cref="Login"/> page renders when no user is logged in.
        /// </summary>
        [Fact]
        public void WhenNoUserIsLogged_RendersLoginButton() {
            Services.AddSingleton<NavigationManager, FakeNavigationManager>();
            var navMenu = RenderComponent<NavMenu>(p => p
                .Add(n => n.UserIsLogged, false));

            var loginButton = navMenu
                .FindAll("li")
                .SingleOrDefault(li => li.Id == nameof(Login));

            Assert.NotNull(loginButton);
        }

        /// <summary>
        /// Tests that the buttons to navigate to <see cref="MyFavorites"/> and to <see cref="Logout"/> pages
        /// render when a user is logged in.
        /// </summary>
        [Fact]
        public void WhenUserIsLogged_RendersFavoritesLogoutButtons() {
            Services.AddSingleton<NavigationManager, FakeNavigationManager>();
            var navMenu = RenderComponent<NavMenu>(p => p
                .Add(n => n.UserIsLogged, true));

            var listItems = navMenu
                .FindAll("li");
            var favoritesButton = listItems.SingleOrDefault(li => li.Id == nameof(MyFavorites));
            var logoutButton = listItems.SingleOrDefault(li => li.Id == nameof(Logout));

            Assert.NotNull(favoritesButton);
            Assert.NotNull(logoutButton);
        }
    }
}