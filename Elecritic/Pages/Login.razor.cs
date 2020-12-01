using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Helpers;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class Login {

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserContext UserContext { get; set; }

        [Inject]
        public UserService UserService { get; set; }

        private UserDto Model { get; set; } = new UserDto();

        private string ResultMessage { get; set; } = "";

        /// <summary>
        /// Queries the database for the password of a user whose email matches the one provided.
        /// If both passwords match, the rest of the user's data is retrieved.
        /// </summary>
        public async Task LogInUser() {
            ResultMessage = "Iniciando sesión...";

            // hash input password
            string hashedPassword = Hasher.GetHashedPassword(Model.Password);
            // get corresponding password from database
            string dbPassword = await UserContext.GetHashedPasswordAsync(Model.Email);

            // an empty password means that the input user doesn't exist
            if (string.IsNullOrEmpty(dbPassword)) {
                ResultMessage =
                    $"Parece que no existe ninguna cuenta con el correo '{Model.Email}'. " +
                    "Intenta crear una nueva ;)";
                return;
            }

            // if both passwords match
            if (hashedPassword == dbPassword) {
                
                // retrieve user from database with all data
                var user = await UserContext.GetUserAsync(Model.Email);
                // update logged in user
                UserService.LogIn(user);

                ResultMessage = "¡Sesión iniciada! :D";
                NavigationManager.NavigateTo("/");
                
                
            }
            else {
                ResultMessage = "Contraseña incorrecta.";
            }
        }

        void GoToSignup() {
            NavigationManager.NavigateTo("/signup");
        }

        public class UserDto {
            [Required]
            [StringLength(50)]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(50)]
            public string Password { get; set; }
        }
    }
}
