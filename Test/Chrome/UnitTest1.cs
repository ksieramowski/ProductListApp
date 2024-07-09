using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using NUnit.Framework.Interfaces;

namespace Test.Chrome {
    public partial class Tests {

        private static string Root;
        private static ChromeDriver Driver;


        [SetUp]
        public void Setup() {
            Root = @"https://localhost:7051/"; // local test

            Driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown() {
            Driver.Close();
            Driver.Dispose();
        }

        [Test]
        public void CreateProductList() {
            TestSeed seed = new();
            CreateProductList(seed);
        }

        public void CreateProductList(TestSeed seed) {
            Login(seed);

            Driver.Navigate().GoToUrl(Root);
            var productsButton = Driver.FindElement(By.Id("pla-nav-products"));
            Assert.That(productsButton, Is.Not.Null);

            productsButton.Click();
            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            IWebElement? tbody = null;
            try { tbody = Driver.FindElement(By.Id("pla-productlists-tbody")); }
            catch {}

            int count = 0;
            ReadOnlyCollection<IWebElement>? lists = null;
            if (tbody != null) {
                lists = tbody.FindElements(By.XPath(".//*"));
                count = lists.Count();
            }

            var listNameInput = Driver.FindElement(By.Id("pla-productlists-new-name"));
            Assert.That(listNameInput, Is.Not.Null);

            var submitButton = Driver.FindElement(By.Id("pla-productlists-new-submit"));
            Assert.That(submitButton, Is.Not.Null);

            string listName = $"list_{seed.Value}";

            listNameInput.SendKeys(listName);
            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            try { tbody = Driver.FindElement(By.Id("pla-productlists-tbody")); }
            catch { }

            Assert.That(tbody, Is.Not.Null);

            lists = tbody.FindElements(By.XPath(".//*"));
            Assert.That(lists.Count(), Is.GreaterThan(count));
        }

        [Test]
        public void AddProduct() {
            TestSeed seed = new();
            AddProduct(seed);
        }

        public void AddProduct(TestSeed seed) {
            CreateProductList(seed);

            var tbody = Driver.FindElement(By.Id("pla-productlists-tbody"));

            Assert.That(tbody, Is.Not.Null);

            var column = Driver.FindElements(By.ClassName("pla-productlists-col-1")).FirstOrDefault();
            Assert.That(column?.TagName, Is.EqualTo("td"));

            var listLink = column.FindElements(By.XPath(".//*")).FirstOrDefault();
            Assert.That(listLink?.TagName, Is.EqualTo("a"));

            listLink.Click();

            //Assert.That(Driver.Url, Does.Contain($"listName=list_{seed.Value}"));

            int count = 0;
            tbody = null;
            try {
                tbody = Driver.FindElement(By.Id("pla-products-tbody"));
                count = tbody.FindElements(By.XPath(".//*")).Count();
            }
            catch {}

            var nameInput = Driver.FindElement(By.Id("pla-products-new-name"));
            var priceInput = Driver.FindElement(By.Id("pla-products-new-price"));
            var quantityInput = Driver.FindElement(By.Id("pla-products-new-quantity"));

            Assert.That(nameInput, Is.Not.Null);
            Assert.That(priceInput, Is.Not.Null);
            Assert.That(quantityInput, Is.Not.Null);

            string name = $"product_{seed.Value}";
            string price = "\b\b\b\b21,37";
            string quantity = "23";

            nameInput.SendKeys(name);
            priceInput.SendKeys(price);
            quantityInput.SendKeys(quantity);

            var submitButton = Driver.FindElement(By.Id("pla-products-new-submit"));
            Assert.That(submitButton, Is.Not.Null);

            submitButton.Click();
            //Assert.That(Driver.Url, Does.Contain($"listName=list_{seed.Value}"));

            tbody = Driver.FindElement(By.Id("pla-products-tbody"));
            Assert.That(tbody.FindElements(By.XPath(".//*")).Count(), Is.GreaterThan(count));

        }

        //[Test]
        //public void AddProduct() {
        //    string productsUrl = Root + "Products";
        //    Driver.Navigate().GoToUrl(productsUrl);
        //    Assert.That(Driver.Url, Is.EqualTo(productsUrl));

        //    var tbody = Driver.FindElement(By.Id("tbody-product-list"));
        //    Assert.That(tbody, Is.Not.Null);
        //    var productsInList = tbody.FindElements(By.XPath(".//*"));
        //    int countBefore = productsInList.Count();

        //    var nameInput = Driver.FindElement(By.Id("input-product-name"));
        //    Assert.That(nameInput, Is.Not.Null);
        //    nameInput.SendKeys("produkt testowy");

        //    var quantityInput = Driver.FindElement(By.Id("input-product-quantity"));
        //    Assert.That(quantityInput, Is.Not.Null);
        //    quantityInput.SendKeys("2");

        //    var priceInput = Driver.FindElement(By.Id("input-product-price"));
        //    Assert.That(priceInput, Is.Not.Null);
        //    priceInput.SendKeys("12");

        //    var button = Driver.FindElement(By.Id("button-add-product"));
        //    Assert.That(button, Is.Not.Null);
        //    button.Click();

        //    Assert.That(Driver.Url, Is.EqualTo(productsUrl));

        //    tbody = Driver.FindElement(By.Id("tbody-product-list"));
        //    Assert.That(tbody, Is.Not.Null);
        //    productsInList = tbody.FindElements(By.XPath(".//*"));
        //    Assert.That(countBefore, Is.LessThan(productsInList.Count()));
        //}

        //[Test]
        //public void AddInvalidProduct_Name() {
        //    string productsUrl = Root + "Products";
        //    Driver.Navigate().GoToUrl(productsUrl);
        //    Assert.That(Driver.Url, Is.EqualTo(productsUrl));

        //    var tbody = Driver.FindElement(By.Id("tbody-product-list"));
        //    Assert.That(tbody, Is.Not.Null);
        //    var productsInList = tbody.FindElements(By.XPath(".//*"));
        //    int countBefore = productsInList.Count();

        //    var nameInput = Driver.FindElement(By.Id("input-product-name"));
        //    Assert.That(nameInput, Is.Not.Null);
        //    //nameInput.SendKeys("produkt testowy");

        //    var quantityInput = Driver.FindElement(By.Id("input-product-quantity"));
        //    Assert.That(quantityInput, Is.Not.Null);
        //    quantityInput.SendKeys("2");

        //    var priceInput = Driver.FindElement(By.Id("input-product-price"));
        //    Assert.That(priceInput, Is.Not.Null);
        //    priceInput.SendKeys("12");

        //    var button = Driver.FindElement(By.Id("button-add-product"));
        //    Assert.That(button, Is.Not.Null);
        //    button.Click();

        //    Assert.That(Driver.Url, Is.EqualTo(productsUrl));

        //    tbody = Driver.FindElement(By.Id("tbody-product-list"));
        //    Assert.That(tbody, Is.Not.Null);
        //    productsInList = tbody.FindElements(By.XPath(".//*"));
        //    Assert.That(countBefore, Is.EqualTo(productsInList.Count()));
        //}
    }
}