namespace ProductListApp.Models {
    public class Products {
        public Product NewProduct { get; set; }
        public IEnumerable<Product> ProductList { get; set; }

        public Products() {
            NewProduct = new Product();
            ProductList = new List<Product>();
        }

        public Products(Product newProduct, IEnumerable<Product> productList) {
            NewProduct = newProduct;
            ProductList = productList;
        }
    }
}
