namespace ProductListApp.Models {
    public class Products {
        public string? ListName { get; set; }

        public Product NewProduct { get; set; }
        public IEnumerable<Product> ProductList { get; set; }

        public Products() {
            ListName = string.Empty;
            NewProduct = new Product();
            ProductList = new List<Product>();
        }

        public Products(string? listName, Product newProduct, IEnumerable<Product> productList) {
            ListName = listName;
            NewProduct = newProduct;
            ProductList = productList;
        }
    }
}
