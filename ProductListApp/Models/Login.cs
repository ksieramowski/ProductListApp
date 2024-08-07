﻿using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models {
    public class Login {
        [Required(ErrorMessage = "RequireEmail")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "RequirePassword")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
