
using Elecritic.Models;
using Elecritic.Services;

using Xunit;

namespace Elecritic.UnitTests {

    public class UserServiceTests : IClassFixture<UserService> {

        /// <summary>
        /// Stub object for the dependency added in <see cref="Startup"/>.
        /// </summary>
        public UserService UserService { get; set; }

        /// <summary>
        /// Sets an empty instance of <see cref="UserService"/> each time this class is tested,
        /// so the test methods share the same instance.
        /// </summary>
        public UserServiceTests(UserService userService) {
            UserService = userService;
        }

        /// <summary>
        /// Tests that <see cref="UserService.LogIn(User)"/> successfully updates <see cref="UserService.LoggedUser"/> with the new instance.
        /// </summary>
        [Fact]
        public void LogIn_UpdatesLoggedUser() {
            var userLoggingIn = new User {
                Id = 1,
                Username = "username",
                Email = "e@mail.com"
            };

            UserService.LogIn(userLoggingIn);

            var expectedLoggedUser = userLoggingIn;
            Assert.Equal(expectedLoggedUser, UserService.LoggedUser);
        }

        /// <summary>
        /// Tests that <see cref="UserService.LogOut"/> successfully updates <see cref="UserService.LoggedUser"/> with an empty instance,
        /// setting its <c>Id</c> to <c>0</c>, meaning that no user is logged in.
        /// </summary>
        [Fact]
        public void LogOut_ResetsLoggedUser() {
            UserService.LogOut();

            int expectedNoLoggedUserId = 0;
            Assert.Equal(UserService.LoggedUser.Id, expectedNoLoggedUserId);
        }
    }
}
