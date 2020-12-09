using Microsoft.AspNetCore.Components;

namespace Elecritic.UnitTests.Fakes {

    /// <summary>
    /// Fake object to replace <see cref="NavigationManager"/> service.
    /// </summary>
    public class FakeNavigationManager : NavigationManager {

        /// <summary>
        /// Initializes the service with dummy links.
        /// </summary>
        public FakeNavigationManager() {
            Initialize("https://fake.link/", "https://fake.link/test/");
        }

        /// <summary>
        /// Do not switch pages as it's a fake object.
        /// </summary>
        protected override void NavigateToCore(string uri, bool forceLoad) {
            Uri = uri;
        }
    }
}