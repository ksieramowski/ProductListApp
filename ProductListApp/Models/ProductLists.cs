namespace ProductListApp.Models {
    public class ProductLists {
        public ProductList NewProductList { get; set; }
        public IEnumerable<ProductList> ListOfProductLists { get; set; }

        public ProductLists() {
            NewProductList = new ProductList();
            ListOfProductLists = new List<ProductList>();
        }

        public ProductLists(ProductList newProductList, IEnumerable<ProductList> listOfProductLists) {
            NewProductList = newProductList;
            ListOfProductLists = listOfProductLists;
        }

    }
}
