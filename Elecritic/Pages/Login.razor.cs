using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Helpers;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class Login {

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserContext UserContext { get; set; }

        private UserDto Model { get; set; } = new UserDto();

        private string ResultMessage { get; set; } = "";

        /// <summary>
        /// Queries the database for the password of a user whose email matches the one provided.
        /// If both passwords match, the rest of the user's data is retrieved.
        /// </summary>
        public async Task LogInUser() {
            ResultMessage = "Iniciando sesión...";

            string hashedPassword = Hasher.GetHashedPassword(Model.Password);
            string dbPassword = await UserContext.GetHashedPasswordAsync(Model.Email);

            if (string.IsNullOrEmpty(dbPassword)) {
                ResultMessage =
                    $"Parece que no existe ninguna cuenta con el correo '{Model.Email}'. " +
                    "Intenta crear una nueva ;)";
                return;
            }

            if (hashedPassword == dbPassword) {
                var user = await UserContext.GetUserAsync(Model.Email);
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
