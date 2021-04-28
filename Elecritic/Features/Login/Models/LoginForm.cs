﻿using System.ComponentModel.DataAnnotations;

namespace Elecritic.Features.Login.Models {
    public class LoginForm {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
