using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elecritic.Pages {
    public partial class Login {

        private LoginRequest Model { get; set; } = new LoginRequest();

        public async Task LoginUser() {

        }

        void GoToRegister() {
            navigationManager.NavigateTo("/register");
        }

        public class LoginRequest {
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
