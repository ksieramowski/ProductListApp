using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models {
    public class Register {

        [Required(ErrorMessage = "RequireName")]
        [Display(Name = "Name")]
        [StringLength(20)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "RequireEmail")]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "RequirePassword")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "RequireConfirmPassword")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        [Compare("Password")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
