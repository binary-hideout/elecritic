using System.Threading.Tasks;

using Elecritic.Features.Users.Commands;
using Elecritic.Features.Users.Models;
using Elecritic.Features.Users.Queries;
using Elecritic.Helpers;
using Elecritic.Models;
using Elecritic.Services;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Users.Pages {
    public partial class Signup {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IMediator Mediator { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        private SignupForm SignupForm { get; set; }

        /// <summary>
        /// Message to display after trying to create a new account.
        /// </summary>
        private string ResultMessage { get; set; }

        private bool IsSigningUp { get; set; }

        public Signup() {
            SignupForm = new SignupForm();
            ResultMessage = "";
            IsSigningUp = false;
        }

        /// <summary>
        /// Calls the database to add the new account.
        /// If it succeeded, the application is redirected to Index, otherwise an error message is displayed.
        /// </summary>
        private async Task SignUpAsync() {
            IsSigningUp = true;
            await Task.Delay(1);
            ResultMessage = "Estamos creando tu nueva cuenta...";

            // create instance of user based on DTO
            var newUser = new User {
                Username = SignupForm.Username,
                Email = SignupForm.Email,
                FirstName = SignupForm.FirstName ?? "",
                LastName = SignupForm.LastName ?? "",
                Password = Hasher.GetHashedPassword(SignupForm.Password),
                Role = (await Mediator.Send(new GetRole.Query { RoleId = 2 })).Role
            };

            if (await Mediator.Send(new Add.Command { User = newUser })) {
                ResultMessage = "¡Cuenta creada con éxito! :D";

                // update logged in user
                await (AuthStateProvider as AuthenticationService).LogIn(newUser);

                NavigationManager.NavigateTo("/");
            }
            else {
                ResultMessage = "Lo sentimos, tu cuenta no pudo ser creada :(";
            }

            IsSigningUp = false;
        }

        private void GoToLogin() {
            NavigationManager.NavigateTo("/login");
        }
    }
}
