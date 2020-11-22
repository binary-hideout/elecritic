using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class Signup {

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private UserDto Model { get; set; } = new UserDto();

        /// <summary>
        /// Method not implemented yet.
        /// </summary>
        /// <returns></returns>
        public async Task SignUpUser() {
            await Task.CompletedTask;
        }

        void GoToLogin() {
            NavigationManager.NavigateTo("/login");
        }

        public class UserDto {
            [Required]
            [StringLength(50)]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(25)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(25)]
            public string LastName { get; set; }

            [Required]
            [StringLength(50)]
            public string Password { get; set; }

            [Required]
            [StringLength(50)]
            [Compare("Password", ErrorMessage ="Password and Confirm password must match")]
            public string ConfirmPassword { get; set; }
        }
    }
}
