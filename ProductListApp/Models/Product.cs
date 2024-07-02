using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductListApp.Models {
    public class Product {
        [Key]
        public int Id { get; set; }

        //public int ProductListId { get; set; }
        //[ForeignKey("ProductListId")]
        //public ProductList? ProductList { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }


    }
}
