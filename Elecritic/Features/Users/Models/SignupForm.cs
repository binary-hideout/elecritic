using System.ComponentModel.DataAnnotations;

namespace Elecritic.Features.Users.Models {
    public class SignupForm {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [StringLength(25)]
        public string FirstName { get; set; }

        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        [Compare(nameof(Password), ErrorMessage = "La contraseña debe coincidir.")]
        public string ConfirmPassword { get; set; }
    }
}
