using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Features.Users.Modules;
using Elecritic.Features.Users.Queries;
using Elecritic.Models;
using Elecritic.Services;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Users.Pages {
    public partial class Login {
        public class Form {
            [Required]
            [StringLength(50)]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(50)]
            public string Password { get; set; }
        }

        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IMediator Mediator { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private Form FormModel { get; set; }
        private User LoggedInUser { get; set; }

        private string ResultMessage { get; set; }

        private bool IsLoggingIn { get; set; }

        public Login() {
            FormModel = new Form();
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
                    new GetLogin.Query {
                        Email = FormModel.Email,
                        Password = Hasher.GetHashedPassword(FormModel.Password)
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
