using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class Login {

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private UserDto Model { get; set; } = new UserDto();

        /// <summary>
        /// Method not implemented yet.
        /// </summary>
        /// <returns></returns>
        public async Task LogInUser() {
            await Task.CompletedTask;
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
