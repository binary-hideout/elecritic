using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elecritic.Pages {
    public partial class Register {

        private RegisterRequest Model { get; set; } = new RegisterRequest();

        public async Task RegisterUser() {

        }

        void GoToLogin() {
            navigationManager.NavigateTo("/login");
        }

        public class RegisterRequest {
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
