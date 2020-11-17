using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elecritic.Pages {
    public partial class LoginAdmin {

        private LoginRequest ModelAdmin { get; set; } = new LoginRequest();

        public async Task LoginUserAdmin() {

        }

        void GoToRegister() {
            navigationManager.NavigateTo("/upload-files");
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
