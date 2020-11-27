using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Helpers;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class Signup {

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserContext UserContext { get; set; }

        [Inject]
        private UserService UserService { get; set; }

        private UserDto Model { get; set; } = new UserDto();

        /// <summary>
        /// Message to display after trying to create a new account.
        /// </summary>
        private string ResultMessage { get; set; } = "";

        /// <summary>
        /// Calls the database to add the new account.
        /// If it succeeded, the application is redirected to Index, otherwise an error message is displayed.
        /// </summary>
        private async Task SignUpUser() {
            ResultMessage = "Estamos creando tu nueva cuenta...";

            // hash input password
            string hashedPassword = Hasher.GetHashedPassword(Model.Password);
            // create instance of user based on DTO
            var newUser = new User {
                Username = Model.Username,
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                Password = hashedPassword
            };

            // try to create new account
            bool signupSucceeded = await UserContext.InsertUserAsync(newUser);
            if (signupSucceeded) {
                ResultMessage = "¡Cuenta creada con éxito! :D";
                // update logged in user
                UserService.LogIn(newUser);
                NavigationManager.NavigateTo("/");
            }
            else {
                ResultMessage = "Lo sentimos, tu cuenta no pudo ser creada :(";
            }
        }

        private void GoToLogin() {
            NavigationManager.NavigateTo("/login");
        }

        public class UserDto {
            [Required]
            [StringLength(50, MinimumLength = 5)]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(20, MinimumLength = 3)]
            public string Username { get; set; }

            [Required]
            [StringLength(25)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(25)]
            public string LastName { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 4)]
            public string Password { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 4)]
            [Compare("Password", ErrorMessage ="Password and Confirm password must match")]
            public string ConfirmPassword { get; set; }
        }
    }
}
