using System.Threading.Tasks;

using Elecritic.Features.Login.Models;
using Elecritic.Features.Login.Queries;
using Elecritic.Helpers;
using Elecritic.Models;
using Elecritic.Services;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Login.Pages {
    public partial class LoginPage {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IMediator Mediator { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private LoginForm LoginForm { get; set; }

        private User LoggedInUser { get; set; }

        private string ResultMessage { get; set; }

        private bool IsLoggingIn { get; set; }

        public LoginPage() {
            LoginForm = new LoginForm();
            ResultMessage = "";
            IsLoggingIn = false;
        }

        protected override async Task OnInitializedAsync() {
            var authState = await AuthStateTask;
            if (authState.User.Identity.IsAuthenticated) {
                LoggedInUser = new User(authState.User);
            }
        }

        /// <summary>
        /// Queries the database for the password of a user whose email matches the one provided.
        /// If both passwords match, the rest of the user's data is retrieved.
        /// </summary>
        public async Task LogInAsync() {
            IsLoggingIn = true;
            await Task.Delay(1);
            ResultMessage = "Iniciando sesión...";

            // hash input password
            var requestedUser = (await Mediator.Send(
                    new GetUser.Query {
                        Email = LoginForm.Email,
                        Password = Hasher.GetHashedPassword(LoginForm.Password)
                    }))
                .UserDto;

            if (requestedUser is not null) {
                var user = new User {
                    Id = requestedUser.Id,
                    Username = requestedUser.Name,
                    Role = requestedUser.Role
                };
                // update logged in user
                await (AuthStateProvider as AuthenticationService).LogIn(user);
                ResultMessage = "¡Sesión iniciada! :D";
                NavigationManager.NavigateTo("/");
            }
            else {
                ResultMessage = "La contraseña es incorrecta o el correo electrónico no existe.";
            }

            IsLoggingIn = false;
        }

        private void GoToSignup() {
            NavigationManager.NavigateTo("/signup");
        }
    }
}
