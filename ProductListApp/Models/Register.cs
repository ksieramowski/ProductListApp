using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models {
    public class Register {

        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
