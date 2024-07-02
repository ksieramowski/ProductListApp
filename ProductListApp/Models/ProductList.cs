using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductListApp.Models {
    public class ProductList {

        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public string? Name { get; set; }

        public DateTime? CreationTime { get; set; }
    }
}
