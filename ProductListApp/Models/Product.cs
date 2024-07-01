using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models {
    public class Product {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
