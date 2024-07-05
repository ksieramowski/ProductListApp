using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductListApp.Models {
    public class Product {
        [Key]
        public int Id { get; set; }

        public int ProductListId { get; set; }
        [ForeignKey("ProductListId")]
        public ProductList? ProductList { get; set; }

        [Required(ErrorMessage = "RequireName")]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "RequirePrice")]
        [RegularExpression(@"^\d{1,6},\d{2}$", ErrorMessage = "InvalidPrice")]
        [Display(Name = "Price")]
        public string? Price { get; set; }

        [Required(ErrorMessage = "RequireQuantity")]
        [Range(1, 999999)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }


        public Product() {
            Price = "0,00";
            Quantity = 1;
            Status = false;
        }
    }
}
