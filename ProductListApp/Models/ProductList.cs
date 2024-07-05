using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductListApp.Models {
    public class ProductList {

        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required(ErrorMessage = "RequireName")]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "CreationTime")]
        public DateTime? CreationTime { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }
    }
}
